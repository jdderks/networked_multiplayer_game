<?php
include"connect.php";

$query = "SELECT * FROM  scores WHERE player_id=1"; // Query = more or less a question (to check something)

if(! ($result = $mysqli->query($query)))
{
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();

do {
    echo $row["player"] . $row["score"] . "<br>";
    } while ($row = $result->fetch_assoc());
?>