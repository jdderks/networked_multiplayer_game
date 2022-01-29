<?php
include "connect.php";

session_start();

$player_id = $_POST["id"];

$sql = "SELECT name FROM users WHERE id = $player_id";

if (!($result = $mysqli->query($sql))) {
    showerror($mysqli->errno, $mysqli->error);
}

while ($row = $result->fetch_assoc()) {
    echo $row["name"];
}

$mysqli->close();