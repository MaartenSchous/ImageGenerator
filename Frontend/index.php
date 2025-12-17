<?php
// Set the content type to HTML
header('Content-Type: text/html; charset=utf-8');

//To-Do Also make a version or other project that uses Sessions instead. This is fine for now so I have a non-session version for reference.
//To-Do add an option to be e-mailed when the requested image is created in the gallery or the ticket is discarded. A good project for when I pick up javascript?

$status_message = "";
$status_class = "";

//When we return here after the prompt check if the 'status' parameter is set in the URL
if (isset($_GET['status'])) {
    $status = $_GET['status'];
    $message = $_GET['message'] ?? 'An unknown issue occurred.';

    if ($status === 'success') {
        //Handle the message
        $status_message = "Successfully requested: " . htmlspecialchars(urldecode($message));
        $status_class = "alert-success";
    } elseif ($status === 'error') {
        $status_message = "Submission Failed: " . htmlspecialchars(urldecode($message));
        $status_class = "alert-error";
    }
}

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Generator Navigation</title>
    <link rel="stylesheet" href="/styles.css">

    <style>
        .alert-container {
            margin: 20px auto;
            padding: 15px;
            border-radius: 8px;
            width: 80%;
            max-width: 600px;
            font-weight: bold;
            text-align: center;
        }
        .alert-success {
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        .alert-error {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
    
</head>
<body>

    <h1>✨ Generator Navigation Hub ✨</h1>

    <?php 
    //Insert the message container if a message is set. If it isn't then this is a new opening of the page and no container is needed.
    if (!empty($status_message)): 
    ?>
    <div class="alert-container <?php echo $status_class; ?>">
        <?php echo $status_message; ?>
    </div>
    <?php endif; ?>

    <div class="button-container">

        <a href="https://maartenschous.nl/" class="button">
            ⬅️ Back
        </a>

        <a href="https://maartenschous.nl/ai/prompt.php" class="button">
            💬 Suggest Prompt
        </a>

        <a href="https://maartenschous.nl/ai/gallery.php" class="button">
            🖼️ Gallery
        </a>

    </div>

</body>
</html>