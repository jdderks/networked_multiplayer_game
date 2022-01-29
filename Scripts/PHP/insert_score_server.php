<?php
include"connect.php";

//Start session
session_start();

//SESSION gets from current running session in browser
$server_id = $_SESSION["server_id"];
$player_id = $_POST["player_id"];

//GET gets from URL
$score =     $_POST["score"];

$query = "INSERT INTO scores (id, server_id, player_id, score, date) VALUES (NULL, $server_id, $player_id, $score, CURRENT_TIMESTAMP())";

if($mysqli->query($query) === TRUE)
{
    echo "1";
} 
else
{
    showerror($mysqli->errno, $mysqli->error);
    echo "0";
}

$mysqli->close();
?>