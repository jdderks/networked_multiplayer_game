<?php
include"connect.php";

//Start session
session_start();

//SESSION gets from current running session in browser
$player_id = $_POST["player_id"];
$logtype = $_POST["logtype"];


$query = "INSERT INTO logs (id, player_id, date, logtype) VALUES (NULL, $player_id, CURRENT_TIMESTAMP(), $logtype)";

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