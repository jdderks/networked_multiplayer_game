<?php
include"connect.php";

session_start();
// echo "Session ID: " . session_id();

$sql = "SELECT id, name, mailadress, password, regdate  FROM users"

if(!($result) = $mysqli->query($sql))
{
    showerror($mysqli->errno,$mysqli->error);
}

$row = $result->fetch_assoc();

echo json_encode($row);

$mysqli->close();
?>