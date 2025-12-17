<?php
require_once 'config.php';
//It loads all entries from the gallery. If the gallery is huge that can become a problem. Using pages to limit the number of entries is an option. But, like most galleries, these should be highlights, not bulk.

// Initialize variables
$entries = [];
$error_message = '';

// =========================================================
// 1. DATABASE CONNECTION AND QUERY
// =========================================================

// Use constants from config.php for connection
$conn = new mysqli(DB_HOST, DB_USER, DB_PASS, DB_NAME);

if ($conn->connect_error) {
    $error_message = " Database Connection Failed: " . $conn->connect_error;
} else {
    // Query to select all required columns from the Images table
    $sql = "SELECT ID, Seed, Prompt, Filelocation FROM Images ORDER BY ID DESC";
    $result = $conn->query($sql);

    if ($result) {
        if ($result->num_rows > 0) {
            // Fetch all entries into an array
            while ($row = $result->fetch_assoc()) {
                $entries[] = $row;
            }
        } else {
            $error_message = "No images found in the database yet. Get prompting!";
        }
        $result->free();
    } else {
        $error_message = "Query Failed: " . $conn->error;
    }

    $conn->close();
}

// =========================================================
// 2. HTML STRUCTURE AND STYLING
// =========================================================

header('Content-Type: text/html; charset=utf-8');
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gallery - Maarten Schous</title>
    <link rel="stylesheet" href="/gallery_styles.css">
</head>
<body>


<div class="gallery-wrapper">
    <h2>Gallery</h2>

        <!-- Thumbnail row -->
    <div class="thumbnail-row">
        <?php foreach ($entries as $index => $entry) : ?>
            <img class="thumb <?php echo $index === 0 ? 'active' : ''; ?>"
                 src="<?php echo htmlspecialchars($entry['Filelocation']); ?>" 
                 alt="Image ID <?php echo htmlspecialchars($entry['ID']); ?>"
                 data-large="<?php echo htmlspecialchars($entry['Filelocation']); ?>" 
                 data-caption="<?php echo htmlspecialchars($entry['Prompt']); ?>">
        <?php endforeach; ?>
    </div>

    <!-- Main display -->
    <div class="main-image-container">
        <?php if (!empty($entries)) : ?>
            <img id="mainImage" src="<?php echo htmlspecialchars($entries[0]['Filelocation']); ?>" 
                 alt="Large display">
            <p id="mainCaption"><?php echo htmlspecialchars($entries[0]['Prompt']); ?></p>
        <?php else : ?>
            <p>No images available.</p>
        <?php endif; ?>
    </div>


</div>

<footer>
    &copy; <?php echo date("Y"); ?> Maarten Schous. All rights reserved. | Site by Maarten Schous.
</footer>

<script>
// JS for thumbnail click -> main image update
const mainImage = document.getElementById("mainImage");
const mainCaption = document.getElementById("mainCaption");
const thumbnails = document.querySelectorAll(".thumb");

thumbnails.forEach(thumb => {
    thumb.addEventListener("click", () => {
        // Update main image and caption
        mainImage.src = thumb.dataset.large;
        mainCaption.textContent = thumb.dataset.caption;

        // Update active thumbnail
        thumbnails.forEach(t => t.classList.remove("active"));
        thumb.classList.add("active");
    });
});
</script>

</body>
</html>
