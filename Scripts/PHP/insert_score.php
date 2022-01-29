<?php
include"connect.php";

//Start session
session_start();

//SESSION gets from current running session in browser
$server_id = $_SESSION["server_id"];
$player_id = $_SESSION["player_id"];
$player =    $_SESSION["player"];

//GET gets from URL
$score =     $_GET["score"];

$query = "INSERT INTO scores (id, server_id, player_id, player, score, date) VALUES (NULL, $server_id, $player_id, '$player', $score, CURRENT_TIMESTAMP())";

if($mysqli->query($query) === TRUE)
{
    echo "NEW RECORD CREATED SUCCESFULLY";
} 
else
{
    showerror($mysqli->errno, $mysqli->error);
}
?>