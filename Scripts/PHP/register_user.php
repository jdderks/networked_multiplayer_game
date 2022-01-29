<?php
include"connect.php";

session_start();
// echo "Session ID: " . session_id();

$username = $_POST["username"];
$password = $_POST["password"];
$email = $_POST["email"];

$password = hash("sha256", $password);

$_SESSION["username"] = $username;
$_SESSION["password"] = $password;
$_SESSION["email"] = $email;

$sql = "SELECT name FROM users WHERE name = ?"; //"name" refers to column "name" in database
$statement = $mysqli->prepare($sql);
$statement->bind_param("s",$username); //s = string

$statement->execute();
$result = $statement->get_result();

if($result->num_rows > 0)
{
    echo "Username is already taken.";
} 
else 
{
    echo "Creating account.";
    $sql = "INSERT INTO users (id, name, password, mailadress, regdate) VALUES (NULL, '$username', '$password', '$email', CURRENT_TIMESTAMP())";
    if($mysqli->query($sql) === TRUE)
    {
        echo "New accound created succesfully.";
    } 
    else 
    {
        showerror($mysqli->errno, $mysqli->error);
    }
}
$mysqli->close();
?>