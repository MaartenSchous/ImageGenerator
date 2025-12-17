<?php
require_once 'config.php';

// 2. HANDLE REDIRECT MESSAGES AND FORM REPOPULATION
$success_message = '';
$error_message = '';

// Read status and message from the URL query string after redirection
$status = $_GET['status'] ?? '';
$message = $_GET['message'] ?? '';

// Sanitize and decode the message
if (!empty($message)) {
    $message = htmlspecialchars(urldecode($message));
    if ($status === 'success') {
        $success_message = $message;
    } else {
        $error_message = $message;
    }
}

//Clear old form data passed back on error
$prompt = $_GET['prompt_text'] ?? '';
$style = $_GET['style'] ?? '';
$requester = $_GET['requester'] ?? '';
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Prompt Submission</title>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script> 
    
    <link rel="stylesheet" href="/styles.css">
</head>
<body>

    <h1>📝 Dream Up Your Image Prompt 🎨</h1>

    <div class="form-container">
        
    <?php if ($success_message): ?>
        <p style="color: green; font-weight: bold; text-align: center; border: 2px solid green; padding: 10px; border-radius: 5px;"><?php echo $success_message; ?></p>
    <?php endif; ?>

    <?php if ($error_message): ?>
        <p style="color: red; font-weight: bold; text-align: center; border: 2px solid red; padding: 10px; border-radius: 5px;"><?php echo $error_message; ?></p>
    <?php endif; ?>

    <form action="request_handler.php" method="POST">
        </form>
</div>

    <div class="form-container">
        
        <?php if ($success_message): ?>
            <p style="color: green; font-weight: bold; text-align: center; border: 2px solid green; padding: 10px; border-radius: 5px;"><?php echo $success_message; ?></p>
        <?php endif; ?>

        <?php if ($error_message): ?>
            <p style="color: red; font-weight: bold; text-align: center; border: 2px solid red; padding: 10px; border-radius: 5px;"><?php echo $error_message; ?></p>
        <?php endif; ?>

        <form action="request_handler.php" method="POST">
            
            <label for="prompt_text">Your Vision (Be Specific!):</label>
            <textarea id="prompt_text" name="prompt_text" required placeholder="A fluffy blue cat riding a rainbow bicycle on the moon..."><?php echo htmlspecialchars($prompt); ?></textarea>

            

            <label for="requester_name">Requester Name:</label>
            <input type="text" id="requester_name" name="requester" required placeholder="Your Name" value="<?php echo htmlspecialchars($requester); ?>">

            <div class="g-recaptcha" data-sitekey="<?php echo RECAPTCHA_SITE_KEY; ?>" style="margin-bottom: 20px;"></div>
            
            <div class="button-group">
                <button type="submit" class="whimsical-button submit-button">
                    ✨ Submit Prompt
                </button>
                
                <a href="https://maartenschous.nl/ai/index.php" class="whimsical-button submit-button">
                    ⬅️ Back
                </a>
            </div>

        </form>
    </div>

</body>
</html>