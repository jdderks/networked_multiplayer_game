<?php
include"connect.php";

session_start();
echo session_id();

$mysqli->close();

?>