using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenManager
{

    public class StableDiffusionInterface
    {
        private readonly HttpClient _client;
        private readonly Random _random;

        public StableDiffusionInterface(HttpClient client, Random random)
        {
            _client = client;
            _random = random;
            _client.Timeout = TimeSpan.FromMinutes(10);
        }


        public async Task<Image> ImageGenerator(StableDiffusionParameters parameters)
        {
            string jsonPayload = JsonSerializer.Serialize(parameters);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //We're making this
            System.Drawing.Image generatedImage = null;

            //The stable diffusion server is here
            string apiUrl = "http://localhost:7860/sdapi/v1/txt2img";


            try
            {
                using (var client = new HttpClient())
                {
                    //Send the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync(); // Read the content first!

                    if (!response.IsSuccessStatusCode)
                    {
                        //The content will contain the JSON error message from the API (e.g., "Sampler not found")
                        MessageBox.Show($"API returned status 422 (Unprocessable Entity).\nServer Error Details: {responseContent}", "API Error");
                        throw new HttpRequestException($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    //If successful, proceed with normal processing:
                    var result = JsonSerializer.Deserialize<StableDiffusionResponse>(responseContent);

                    //Read and Parse the Response
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonDocument.Parse(jsonResponse);

                    // Get image and parameters from the response
                    //!Always using batch = 1 as set in the default. Else the next line might be weird
                    string base64Image = apiResponse.RootElement.GetProperty("images")[0].GetString();

                    //Convert Base64 string to an image
                    byte[] imageBytes = Convert.FromBase64String(base64Image);
                    var ms = new MemoryStream(imageBytes);
                    generatedImage = System.Drawing.Image.FromStream(ms);

                    return generatedImage;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API Request Error: {ex.Message}. Check if AUTOMATIC1111 is running with --api flag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }






    public class StableDiffusionParametersResponse
    {
        public StableDiffusionParameters result { get; set; }
        public string imageID { get; set; }
    }

    public class ApiInfo
    {
        public long seed { get; set; }
    }

    public class StableDiffusionParameters
    {
        // --- Core Generation Parameters ---
        public string prompt { get; set; } = "";
        public string negative_prompt { get; set; } = "";

        // API uses an array of strings for styles
        //public List<string> styles { get; set; } = new List<string>();

        public long seed { get; set; } = -1;
        public long subseed { get; set; } = -1;
        public double subseed_strength { get; set; } = 0;
        public int seed_resize_from_h { get; set; } = 0; // -1 is often used, but 0 is safe/default
        public int seed_resize_from_w { get; set; } = 0; // -1 is often used, but 0 is safe/default

        public string sampler_name { get; set; } = "Euler"; // Common default
                                                            //public string scheduler { get; set; } = ""; // Can be set if sampler name is not sufficient

        public int batch_size { get; set; } = 1;
        public int n_iter { get; set; } = 1;
        public int steps { get; set; } = 30;
        public double cfg_scale { get; set; } = 7;
        public int width { get; set; } = 512;
        public int height { get; set; } = 512;

        // --- Post-processing & Utility ---
        public bool restore_faces { get; set; } = false;
        public bool tiling { get; set; } = false;
        // / public bool do_not_save_samples { get; set; } = false;
        /// public bool do_not_save_grid { get; set; } = false;

        // --- Sampler Controls (Sigma parameters) ---
        public double eta { get; set; } = 0;
        public double denoising_strength { get; set; } = 1; // Only relevant if enable_hr is true or for img2img
        public double s_min_uncond { get; set; } = 0;
        public double s_churn { get; set; } = 0;
        public double s_tmax { get; set; } = 0;
        public double s_tmin { get; set; } = 0;
        public double s_noise { get; set; } = 1;

        // --- Settings Overrides ---
        public OverrideSettings override_settings { get; set; } = new OverrideSettings();
        // public bool override_settings_restore_afterwards { get; set; } = true;

        // --- Refiner Parameters ---
        // public string refiner_checkpoint { get; set; } = "";
        // public double refiner_switch_at { get; set; } = 0;
        // public bool disable_extra_networks { get; set; } = false;

        //public string firstpass_image { get; set; } = "";
        //public object comments { get; set; } = null; // Use null for empty dictionary

        // --- High-Resolution Fix (Hires. fix) Parameters ---
        public bool enable_hr { get; set; } = false;
        public int firstphase_width { get; set; } = 0;
        public int firstphase_height { get; set; } = 0;
        public double hr_scale { get; set; } = 2.0;
        public string hr_upscaler { get; set; } = "Latent"; // Common default upscaler
        public int hr_second_pass_steps { get; set; } = 0;
        public int hr_resize_x { get; set; } = 0;
        public int hr_resize_y { get; set; } = 0;
        public string hr_checkpoint_name { get; set; } = "";
        public string hr_sampler_name { get; set; } = "";
        public string hr_scheduler { get; set; } = "";
        public string hr_prompt { get; set; } = "";
        public string hr_negative_prompt { get; set; } = "";

        // --- Scripting & Output Controls ---
        // public string force_task_id { get; set; } = "";
        //public string sampler_index { get; set; } = "Euler"; // Redundant with sampler_name, but included
        //public string script_name { get; set; } = "";
        // public List<object> script_args { get; set; } = new List<object>();

        //public bool send_images { get; set; } = true; // Crucial for getting the result back
        //public bool save_images { get; set; } = false; // Prevents saving files on the server
        //public object alwayson_scripts { get; set; } = null;
        //public string infotext { get; set; } = ""; // Not typically sent in a request
    }

    // Nested classes for overrides
    public class OverrideSettings
    {
        // You should add any setting you plan to override here.
        public string sd_model_checkpoint { get; set; } = "";
        public int CLIP_stop_at_last_layers { get; set; } = 1;
        public int eta_noise_seed_delta { get; set; } = 0;
    }

    // Class for deserializing the API response
    public class StableDiffusionResponse
    {
        public List<string> images { get; set; }
        public object parameters { get; set; }
        public string info { get; set; }
    }





}













