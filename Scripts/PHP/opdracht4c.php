<?php
include"connect.php";

$query = "SELECT id, player_id, MAX(score) as highscore FROM scores WHERE server_id=1"; 

if(! ($result = $mysqli->query($query)))
{
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();

echo json_encode($row)

?>