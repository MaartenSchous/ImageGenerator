-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Dec 17, 2025 at 12:00 PM
-- Server version: 8.0.43
-- PHP Version: 8.2.29

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `imagegenerator`
--

-- --------------------------------------------------------

--
-- Table structure for table `tickets`
--

CREATE TABLE `tickets` (
  `imageid` int NOT NULL,
  `prompt` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `negativeprompt` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `steps` int NOT NULL,
  `width` int NOT NULL,
  `height` int NOT NULL,
  `seed` int NOT NULL,
  `cfgscale` int NOT NULL,
  `samplername` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `batchsize` int NOT NULL,
  `hiresfix` tinyint(1) NOT NULL,
  `hiresupscaler` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `hiressteps` int NOT NULL,
  `hiresdenoisingstrength` float NOT NULL,
  `hiresupscaleby` int NOT NULL,
  `status` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `tier` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `safe` tinyint(1) NOT NULL DEFAULT '0',
  `location` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `origin` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tickets`
--
ALTER TABLE `tickets`
  ADD PRIMARY KEY (`imageid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tickets`
--
ALTER TABLE `tickets`
  MODIFY `imageid` int NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
