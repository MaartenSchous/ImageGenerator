<?php

// 1. INCLUDE CONFIGURATION AND SETUP
require_once 'config.php';

// Set headers to prevent caching and ensure no HTML is sent accidentally
header("Expires: Tue, 01 Jan 2000 00:00:00 GMT");
header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");
header("Cache-Control: no-store, no-cache, must-revalidate, max-age=0");
header("Cache-Control: post-check=0, pre-check=0", false);
header("Pragma: no-cache");

// Check if the request is a POST submission (i.e., the form was submitted), else go back to the index
if ($_SERVER["REQUEST_METHOD"] !== "POST") {
    header("Location: " . REDIRECT_URL);
    exit;
}

$error_message = "";
$ip = $_SERVER['REMOTE_ADDR']; //automatically provided by Web Server
$style = 'Disabled';

// =========================================================
// 2. CAPTCHA VALIDATION
// =========================================================
$recaptchaResponse = $_POST['g-recaptcha-response'] ?? '';

if (empty($recaptchaResponse)) {
    $error_message = "You didn't check the 'I'm not a robot' box. Very suspicious.";
} else {
    // Use the secret key defined in config.php
    $secretKey = RECAPTCHA_SECRET_KEY; 
    $verifyUrl = 'https://www.google.com/recaptcha/api/siteverify';
    
    $data = [
        'secret'   => $secretKey,
        'response' => $recaptchaResponse,
        'remoteip' => $ip
    ];
    
    // Send request to Google using a stream context
    $context = stream_context_create([
        'http' => [
            'header'  => "Content-type: application/x-www-form-urlencoded\r\n",
            'method'  => 'POST',
            'content' => http_build_query($data),
        ],
    ]);
    $response = @file_get_contents($verifyUrl, false, $context);
    $result = json_decode($response, true);
    
    if (!$result || !$result['success']) {
        $error_message = "CAPTCHA verification failed. Are you... A robot?";
    }
}

// =========================================================
// 3. SERVER-SIDE DATA VALIDATION (Anti-Abuse Checks)
// =========================================================
$prompt = trim($_POST['prompt_text'] ?? '');
$requester = trim($_POST['requester'] ?? '');

if (empty($error_message)) { // Only validate form data if CAPTCHA passed
    

    
    // 3b. Check Prompt Length (Max 1000 characters)
    if (strlen($prompt) > 1000) {
         $error_message = "The prompt is too long. Please limit it to 1000 characters.";
    }
    

    // 3d. Check Requester Length (Max 50 characters)
    else if (strlen($requester) > 50) {
         $error_message = "Requester name is too long (Max 50 characters).";
    }
}

// =========================================================
// 4. DATABASE INSERTION (Only proceed if NO errors occurred)
// =========================================================
$success_message = "";
if (empty($error_message)) {
    
    // Use constants from config.php for connection
    $conn = new mysqli(DB_HOST, DB_USER, DB_PASS, DB_NAME);
    
    if ($conn->connect_error) {
        // Log the technical error, give generic error to user
        error_log("DB Connection Error: " . $conn->connect_error);
        $error_message = "A server error occurred. Please try again shortly.";
    } else {
        
        // Use Prepared Statements for security
        $stmt = $conn->prepare("INSERT INTO Requests (prompt, style, requester, requester_ip, submission_date) VALUES (?, ?, ?, ?, NOW())");
        
        // Bind parameters: s=string
        $stmt->bind_param("ssss", $prompt, $style, $requester, $ip);
        
        if ($stmt->execute()) {
            // SUCCESS!
            $success_message = " Prompt submitted successfully! Thank you.";
        } else {
            error_log("DB Insert Error: " . $stmt->error);
            $error_message = "A server error occurred during data storage. Please try again.";
        }
        
        $stmt->close();
        $conn->close();
    }
}

// =========================================================
// 5. REDIRECTION
// =========================================================

// Determine the message to pass in the URL
if (!empty($success_message)) {
    // Success: Redirect with a success parameter
    header("Location: " . "https://maartenschous.nl/ai/index.php" . "?status=success&message=" . urlencode($success_message));
} else {
    // Failure: Redirect with an error parameter and the error message
    // Passing more info than needed, could be useful later.
    $data_params = http_build_query([
        'prompt_text' => $prompt,
        'style' => $style,
        'requester' => $requester
    ]);
    header("Location: " . REDIRECT_URL . "?status=error&message=" . urlencode($error_message) . "&" . $data_params);
}
exit;
?>