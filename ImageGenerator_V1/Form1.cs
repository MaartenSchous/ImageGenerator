using _07_Toolset;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using static _07_Toolset.Toolset;
using GenManager;

//Progression log:
/*
v0.1 - New form and project from old projects without all test code
Integrated Toolset v2.7
Test button with mock generation call to local Stable Diffusion API works as intended

To get to a working v1.0 checklist:
-Generator
-Logging
-Database connectivity to webhost database queue
+Click datagrid should select row and populate prompt box and image box
*/

namespace ImageGenerator_V1
{
    public partial class Form1 : Form
    {
        //Todo move to config
        private const string SaveDirectory = "D:\\Renders\\Engine";

        //Instantiate the toolset
        private Toolset oToolset = new Toolset();

        private static string? MySQLConnectionString;
        public StableDiffusionInterface oStableDiffusionInterface = new StableDiffusionInterface(new HttpClient(), new Random());

        //Webhost
        public int WebhostTickets = 0; //number of pending webhost tickets, they receive priority

        //Standard
        public int StandardTickets = 0; //number of pending standard tickets

        //Datagrid1 binding list
        private readonly BindingList<TicketParameters> _evaluationList;

        //Currently active datagrid row
        DataGridViewRow ActiveRow = null;

        private bool isRunning = false; //toggels loop on/off, based on UI input

        //TODO Image generator instance marks an entry status field as Loaded when it picks it up, so if the app crashes it won't keep trying the same entry.
        //Can add a check at runtime whether there are any entries marked 'Loaded' and reset them to Queued on startup, or an extra tier field value "Stuck" or similar.

        //Considerations:
        //The tickets do not have previews, previews can be generated ahead and added to each ticket but that slowls loading, currently generating as we go instead. 
        //Both options are valid, weighing speed on load vs speed on click.

        //Queues:
        //STANDARD - Personally generated tickets that aren't from the WebhostQueue. Can be discarded, re-generated, or sent to gallery.
        //WEBHOSTQUEUE - A ticket request or result that needs manual appraisal. Can be accepted, discarded, re-generated, or sent to gallery.
        //Gallery - Uploaded to webhost, visible in gallery.

        //Status:
        //Queued - waiting to be generated
        //Processing - Locked for generation
        //Completed - Generated


        public Form1()
        {
            InitializeComponent();
            oToolset.LoadMySQLConfigAsync();
            oToolset.LoadWebhostConfigAsync();
            PopulateUI();

            //Datagrid1
            _evaluationList = new BindingList<TicketParameters>();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _evaluationList;
            dataGridView1.Columns["imageid"].DisplayIndex = 0;
        }

        public void RowIsSelected(DataGridViewRow selectedRow)
        {
            //when a row is selected, populate the prompt box, origin box and image box
            try
            {
                object promptValue = selectedRow.Cells["Prompt"].Value;
                object Origin = selectedRow.Cells["Origin"].Value;
                object Location = selectedRow.Cells["Location"].Value;

                // Set the TextBox text, handling null or empty values safely.
                textBoxPrompt.Text = promptValue != null ? promptValue.ToString() : string.Empty;
                textBoxOrigin.Text = "Origin: " + Origin != null ? Origin.ToString() : string.Empty;
               // string fileLocation = Location.ToString();

                //if there is no image, use placeholder.
                if (Location == null)
                {
                    pictureBox1.Image = Image.FromFile("Ticketonly.png");

                }
                else
                {
                    Image currentImage = Image.FromFile(Location.ToString());
                    pictureBox1.Image = Image.FromFile("TicketOnly.png");
                    Image resizedImage = Toolset.ResizeImage(currentImage, pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.Image = resizedImage;
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur, like no local image found
            }

        }

        public void PopulateUI()
        {
            var entries = oToolset.GetTiers();
            foreach (var item in entries)
            {
                comboBoxTier.Items.Add(item);
            }
        }


        private async void buttonRunPause_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false; // Set the flag to false to stop the loop
                labelRunPause.Text = "Complete and pause"; // Update UI label
                // The loop will exit on its next iteration check
                labelRunPause.BackColor = Color.Yellow;
            }
            else
            {
                // 2. RUN Logic
                isRunning = true; // Set the flag to true to start the loop
                labelRunPause.Text = "Running"; // Update UI label
                labelRunPause.BackColor = Color.LightGreen;

                // Start the long-running loop asynchronously
                await RunOperationsLoop();
            }
        }

        private async Task RunOperationsLoop()
        {
            // The loop continues as long as the status label indicates "Running".
            while (labelRunPause.Text == "Running")
            {
                //Update the UI status labels, and priority count
                updateUIStatus();

                if (WebhostTickets < 1 && StandardTickets < 1) //empty queue
                {


                    labelRunPause.Text = "Empty, paused";

                    labelRunPause.BackColor = Color.Orange;
                    await Task.Delay(5000);
                    labelRunPause.Text = "Running";
                    labelRunPause.BackColor = Color.LightGreen;



                }
                else if (WebhostTickets > 0)
                {
                    labelRunPause.BackColor = Color.LightGreen;
                    await GenerateNext();
                }
                else
                {
                    labelRunPause.BackColor = Color.LightGreen;
                    await GenerateNext();
                }

                   

            }

            //When the Generator pause is clicked (because labelRunPause.Text is no longer "Running"),
            if (labelRunPause.Text != "Running")
            {
                // Ensure the button state reflects that the process has completely stopped, not just paused.
                labelRunPause.Text = "Generator Stopped";
                labelRunPause.BackColor = Color.Orange;
            }
        }

        public async void updateUIStatus()
        {
            //TOdo this would be a good spot to get the ticket requests from the server database

            labelWebhostQueue.Text = "Webhost Queue " + oToolset.CountQueuedEntries("tier", "WebhostQueue").ToString();
            WebhostTickets = oToolset.CountQueuedEntries("tier", "WebhostQueue"); //Priority Queue
            labelWebhostEvaluationQueue.Text = "Webhost Evaluation Queue " + oToolset.CountQueuedEntries("tier", "WebhostEvaluationQueue").ToString();
            labelLocalQueue.Text = "Local Queue " + oToolset.CountQueuedEntries("tier", "STANDARD").ToString();
            labelLocalEvaluationQueue.Text = "Local Evaluation Queue " + oToolset.CountQueuedEntries("tier", "LocalEvaluationQueue").ToString();
            StandardTickets = oToolset.CountQueuedEntries("tier", "STANDARD"); //Standard Queue


        }

        private async void buttonOpenThisQueue_Click(object sender, EventArgs e)
        {
            //Empty it out first
            _evaluationList.Clear();

            // Check for connectivity or general availability (optional but good practice)
            await Task.Delay(10);

            //To-Do check if empty
            string tier = comboBoxTier.Text.ToString();

            //Retrieve all entries from the database from this tier
            List<TicketParameters> parameters = oToolset.GetAllFromTier(tier);

            //can only happen if comboBoxTier was populated and then data was changed, but just in case
            if (parameters == null)
            {
                MessageBox.Show("No entries from that Tier found in the database.",
                                "Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (TicketParameters Queryresult in parameters)
            {
                _evaluationList.Insert(0, Queryresult);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Check if there is at least one row selected.
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the first selected row (even if multiple are selected).
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Ensure the row is not the header row and has the data it needs.
                if (!selectedRow.IsNewRow)
                {
                    ActiveRow = selectedRow;
                    RowIsSelected(selectedRow);
                }
            }
        }

        private void buttonApproveforgeneration_Click(object sender, EventArgs e)
        {

            ActiveRow.Cells["tier"].Value = "Ticket";

            string ConnectionString = oToolset.MySQLConnectionString;

            var newticket = new TicketParameters
            {
                ImageID = Convert.ToInt32(ActiveRow.Cells["ImageID"].Value),
                Status = ActiveRow.Cells["Status"].Value.ToString(),
                Tier = "STANDARD"
            };

            //update the entry in the DB
            oToolset.UpdateStatusAndTier(newticket, ConnectionString);

            //remove handled entry from list
            _evaluationList.RemoveAt(ActiveRow.Index);
        }

        private void buttonQueueAgainNS_Click(object sender, EventArgs e)
        {
            //format data
            TicketParameters newTicket = rowToTicketParameters(ActiveRow);

            //alter some parameters
            newTicket.Tier = "STANDARD";
            newTicket.Status = "Queued Repeat";
            newTicket.Seed = -1;
            newTicket.ImageID = 0;
            newTicket.Location = null;

            //Insert new entry
            oToolset.InsertEntry(newTicket);
        }

        private TicketParameters rowToTicketParameters(DataGridViewRow row)
        {
            TicketParameters newTicket = new TicketParameters
            {
                Prompt = row.Cells["Prompt"].Value.ToString(),
                NegativePrompt = row.Cells["Negativeprompt"].Value.ToString(),
                Steps = Convert.ToInt32(row.Cells["Steps"].Value),
                Width = Convert.ToInt32(row.Cells["Width"].Value),
                Height = Convert.ToInt32(row.Cells["Height"].Value),
                Seed = Convert.ToInt64(row.Cells["Seed"].Value),
                CfgScale = Convert.ToSingle(row.Cells["CfgScale"].Value),
                SamplerName = row.Cells["SamplerName"].Value.ToString(),
                BatchSize = Convert.ToInt32(row.Cells["BatchSize"].Value),
                HiresFix = Convert.ToBoolean(row.Cells["HiresFix"].Value),
                HiresUpscaler = row.Cells["HiresUpscaler"].Value.ToString(),
                HiresSteps = Convert.ToInt32(row.Cells["HiresSteps"].Value),
                HiresDenoisingStrength = Convert.ToSingle(row.Cells["HiresDenoisingStrength"].Value),
                HiresUpscaleBy = Convert.ToSingle(row.Cells["HiresUpscaleBy"].Value),
                ImageID = Convert.ToInt32(row.Cells["ImageID"].Value),
                Status = row.Cells["Status"].Value.ToString(),
                Tier = row.Cells["Tier"].Value.ToString(),
                Safe = Convert.ToBoolean(row.Cells["Safe"].Value),
                Location = row.Cells["Location"].Value.ToString(),
                Origin = row.Cells["Origin"].Value.ToString()
            };

            return newTicket;


        }

        private void buttonDiscardTicket_Click(object sender, EventArgs e)
        {
            //TODO deleting last entry causes index error

            int deletedIndex = ActiveRow.Index;
            //remove entry from SQL database
            string entry = ActiveRow.Cells["ImageID"].Value.ToString();
            bool result = oToolset.DeleteEntry(entry);

            if (result)
            {
                //Remove handled entry from the list (this automatically updates the DataGridView)
                _evaluationList.RemoveAt(deletedIndex);

                // The number of rows remaining after the deletion
                int remainingRowCount = dataGridView1.Rows.Count;

                if (remainingRowCount == 0)
                {
                    //The list is now empty, nothing to select
                    return;
                }

                //Determine the index to select next.
                int newSelectedIndex = (deletedIndex >= remainingRowCount)
                    ? (remainingRowCount - 1)
                    : deletedIndex;

                //Clear any existing selection
                dataGridView1.ClearSelection();

                //Select the determined row and set it as the current cell
                dataGridView1.Rows[newSelectedIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[newSelectedIndex].Cells[0];
            }
            else
            {
                // Optional: Handle the case where SQL deletion failed
                MessageBox.Show("Failed to delete the entry from the database.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            // dataGridView1.CurrentCell = dataGridView1.Rows[ActiveRow.Index].Cells[0];


        }

        private void buttonUploadToWebhost_Click(object sender, EventArgs e)
        {
            //ToDO what if no row is selected?

            // Get the selected row
            DataGridViewRow row = dataGridView1.SelectedRows[0];

            //format data
            TicketParameters newTicket = rowToTicketParameters(row);

            //Try to upload to gallery
            //ToDo wrap in a task to track success, failure, progress etc
            oToolset.UploadToWebhost(newTicket);

            //if that works, local changes
            //alter some parameters
            newTicket.Tier = "Gallery";
            newTicket.Status = "Uploaded";

            //Insert new entry
            oToolset.InsertEntry(newTicket);

            oToolset.UpdateStatusAndTier(newTicket, oToolset.MySQLConnectionString);
            _evaluationList.RemoveAt(ActiveRow.Index);
        }

        //Generate the next ticket in the queue
        public async Task<bool> GenerateNext()
        {
            // Check for connectivity or general availability
            await Task.Delay(10);

            //We're making this
            Image generatedImage;

            //Contained in this
            var returns = new StableDiffusionParametersResponse();

            //Tracking the imageID
            int imageID;

            string tier;

            //if there are webhost tickets, process those first.
            if (WebhostTickets > 0)
            {

                returns = oToolset.GetNextQueuedEntry("WEBHOSTQUEUE");
                tier = "WEBHOSTQUEUE";

            }
            else
            {
                returns = oToolset.GetNextQueuedEntry("STANDARD");
                tier = "STANDARD";
            }

            //Call Stable Diffusion and generate the image
            var ImageGeneratorResponce = await oStableDiffusionInterface.ImageGenerator(returns.result);
            //To-Do error handling if the generation fails

            var Savename = returns.imageID;
            string saveHere = "D:\\Renders\\Engine\\test\\" + Savename + ".png";
            ImageGeneratorResponce.Save(saveHere);

            //local MySQL changes
TicketParameters newTicket = new TicketParameters
{
    Prompt = returns.result.prompt.ToString(),
    NegativePrompt = returns.result.negative_prompt.ToString(),
    SamplerName = returns.result.sampler_name.ToString(),
    HiresUpscaler = returns.result.hr_upscaler.ToString(),
    Status = "Complete",
    Tier = tier,
    Location = saveHere.ToString(),
    //Origin = returns.result.origin

    // Integers
    Steps = returns.result.steps,
    Width = returns.result.width,
    Height = returns.result.height,
    BatchSize = returns.result.batch_size,
    HiresSteps = returns.result.hr_second_pass_steps,
    ImageID = Convert.ToInt32(returns.imageID),

    Seed = returns.result.seed as long?,

    // Floats
    //CfgScale = returns.result.cfg_scale,
    HiresDenoisingStrength = (float)returns.result.denoising_strength,
    HiresUpscaleBy = 2.0f,

    HiresFix = returns.result.enable_hr,

    Safe = false,

};


            oToolset.UpdateStatusTierLocation(newTicket, oToolset.MySQLConnectionString);

            return true;
        }

    }
}
