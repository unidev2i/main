-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Client :  127.0.0.1
-- Généré le :  Ven 01 Avril 2016 à 08:28
-- Version du serveur :  5.6.17
-- Version de PHP :  5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données :  `mydb`
--

-- --------------------------------------------------------

--
-- Structure de la table `classe`
--

CREATE TABLE IF NOT EXISTS `classe` (
  `idClasse` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Niveau` enum('seconde','premiere','terminale','') CHARACTER SET utf8 NOT NULL,
  `numClasse` tinyint(3) unsigned NOT NULL COMMENT 'ex: 3 => seconde  3',
  PRIMARY KEY (`idClasse`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COMMENT='une ligne pour chaque classe' AUTO_INCREMENT=3 ;

--
-- Contenu de la table `classe`
--

INSERT INTO `classe` (`idClasse`, `Niveau`, `numClasse`) VALUES
(1, 'seconde', 1),
(2, 'seconde', 2);

-- --------------------------------------------------------

--
-- Structure de la table `eleve`
--

CREATE TABLE IF NOT EXISTS `eleve` (
  `idEleve` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom` varchar(30) NOT NULL COMMENT 'Nom de l''élève',
  `Prenom` varchar(30) NOT NULL COMMENT 'Prénom de l''élève',
  `idClasse` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`idEleve`),
  KEY `idClasse` (`idClasse`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque élève' AUTO_INCREMENT=3 ;

--
-- Contenu de la table `eleve`
--

INSERT INTO `eleve` (`idEleve`, `Nom`, `Prenom`, `idClasse`) VALUES
(2, 'Daniel', 'jack', 2);

-- --------------------------------------------------------

--
-- Structure de la table `note`
--

CREATE TABLE IF NOT EXISTS `note` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `idPdf` int(10) unsigned NOT NULL,
  `idCompetence` varchar(10) NOT NULL COMMENT 'ex: cp2.2 ; autonomie ; ...',
  `Note` varchar(10) NOT NULL,
  `maxNote` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idTp` (`idPdf`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque note' AUTO_INCREMENT=9 ;

--
-- Contenu de la table `note`
--

INSERT INTO `note` (`ID`, `idPdf`, `idCompetence`, `Note`, `maxNote`) VALUES
(3, 2, 'CP2.3', '9', '11'),
(4, 2, 'CP2.4', '2.5', '3.5'),
(5, 4, 'CP2.3', '5', '10'),
(6, 2, 'CP2.5', '6', '6'),
(7, 4, 'CP2.3', '42', '55'),
(8, 4, 'CP2.3', '25', '47');

-- --------------------------------------------------------

--
-- Structure de la table `tp`
--

CREATE TABLE IF NOT EXISTS `tp` (
  `idPdf` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idTp` varchar(10) NOT NULL COMMENT 'ex: 2 => TP2',
  `idEleve` int(10) unsigned NOT NULL,
  `idcorrecteur` int(10) unsigned NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`idPdf`),
  KEY `idPdf` (`idPdf`),
  KEY `idEleve` (`idEleve`),
  KEY `idcorrecteur` (`idcorrecteur`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque PDF de lu' AUTO_INCREMENT=6 ;

--
-- Contenu de la table `tp`
--

INSERT INTO `tp` (`idPdf`, `idTp`, `idEleve`, `idcorrecteur`, `date`) VALUES
(2, '4', 2, 1, '2016-03-02'),
(4, '2', 2, 1, '2016-03-10'),
(5, '2', 2, 1, '2016-03-31');

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE IF NOT EXISTS `user` (
  `idUser` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Login` varchar(32) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Admin` tinyint(1) DEFAULT '0' COMMENT 'si 1 => admin',
  PRIMARY KEY (`idUser`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque utilisateur (prof)' AUTO_INCREMENT=5 ;

--
-- Contenu de la table `user`
--

INSERT INTO `user` (`idUser`, `Login`, `Password`, `Admin`) VALUES
(1, 'Atogue', 'azerty', 1),
(3, 'noadmin', 'root', 0),
(4, '', '', 1);

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `eleve`
--
ALTER TABLE `eleve`
  ADD CONSTRAINT `fk_classe_eleve` FOREIGN KEY (`idClasse`) REFERENCES `classe` (`idClasse`) ON UPDATE CASCADE;

--
-- Contraintes pour la table `note`
--
ALTER TABLE `note`
  ADD CONSTRAINT `fk_note_tp` FOREIGN KEY (`idPdf`) REFERENCES `tp` (`idPdf`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `tp`
--
ALTER TABLE `tp`
  ADD CONSTRAINT `fk_tp_eleve` FOREIGN KEY (`idEleve`) REFERENCES `eleve` (`idEleve`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
