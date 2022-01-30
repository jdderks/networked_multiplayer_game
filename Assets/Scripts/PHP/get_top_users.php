<?php
include "connect.php";

session_start();
// echo "Session ID: " . session_id() . "<br>";

$limit = $_POST["limit"];

$sql = "SELECT player_id, score from scores ORDER BY score DESC LIMIT $limit";

if (!($result = $mysqli->query($sql))) {
    showerror($mysqli->errno, $mysqli->error);
}

while ($row = $result->fetch_assoc()) {
    echo $row["player_id"] . ",";
}

$mysqli->close();