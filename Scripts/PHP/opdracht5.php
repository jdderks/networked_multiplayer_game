<?php
include"connect.php";

//GET gets from URL
$server_id = $_GET["server_id"];
$player_id = $_GET["player_id"];
$player = $_GET["player"];
$score = $_GET["score"];

$query = "INSERT INTO scores (id, server_id, player_id, player, score, date) VALUES (NULL, $server_id, $player_id, $player, $score, CURRENT_TIMESTAMP())";

if($mysqli->query($query) === TRUE)
{
    echo "NEW RECORD CREATED SUCCESFULLY";
} 
else 
{
    showerror($mysqli->errno, $mysqli->error);
}

//https://studentdav.hku.nl/~joris.derks/networking/opdracht5.php?server_id=1&player_id=9&player="YeetusDingus"&score=222
?>