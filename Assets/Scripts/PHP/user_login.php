<?php
include"connect.php";

session_start();
// echo "Session ID: " . session_id();

$username = $_POST["username"];
$password = $_POST["password"];

$password = hash("sha256", $password);

$_SESSION["username"] = $username;
$_SESSION["password"] = $password;

$sql = "SELECT password FROM users WHERE name = ?";
$statement = $mysqli->prepare($sql);
$statement->bind_param("s",$username); //s = string

$statement->execute();
$result = $statement->get_result();

if($result->num_rows > 0)
{
    while($row = $result->fetch_assoc()) 
    {
        if($row["password"] == "$password")
        {
            $sql = "SELECT id, name, mailadress, regdate FROM users WHERE name = ?";
            $statement = $mysqli->prepare($sql);
            $statement->bind_param("s", $username);

            $statement->execute();
            $result = $statement->get_result();

            $row = $result->fetch_assoc();

            echo json_encode($row);
        }
        else 
        {
            echo "Incorrect username/password.";
        }
    }
} 
else 
{
    echo "Incorrect username/password. (DEBUG: User does not exist)";
}

?>