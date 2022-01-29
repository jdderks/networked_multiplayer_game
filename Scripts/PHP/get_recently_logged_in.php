<?php
include"connect.php";

session_start();
// echo "Session ID: " . session_id();

$date = date('m/d/Y h:i:s');
$dateMinusOneMonth = date('m/d/Y h:i:s a', strtotime("-1 month"));
$counter = 0;

$sql = "SELECT date FROM logs WHERE logtype = 0";

if(!($result = $mysqli->query($sql)))
{
    showerror($mysqli->errno,$mysqli->error);
}

while($row = $result->fetch_assoc())
{
    if($row["date"] >= $dateMinusOneMonth)
    {
        $counter++;
    }
}

echo $counter;

$mysqli->close();
?>