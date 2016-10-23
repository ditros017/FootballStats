﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballStats.Data.Infrastructure;
using FootballStats.Web.Models.Team;

namespace FootballStats.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult List()
        {
            return View("~/Views/Teams/List.cshtml", new ListModel
            {
                Teams = _unitOfWork.TeamRepository.GetAll(t => new ListModel.Team
                {
                    Id = t.Id,
                    Name = t.Name,
                    Country = t.Country,
                    City = t.City,
                    PlayerCount = t.Players.Count
                }).ToArray()
            });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _unitOfWork.TeamRepository.GetById(id, t => new DetailsModel
            {
                Id = t.Id,
                Name = t.Name,
                Country = t.Country,
                City = t.City,
                PlayerCount = t.Players.Count,
                FootballMatchCount = t.FootballMatchTeams.Count,
                Coach = new DetailsModel.CoachModel
                {
                    Id = t.CoachId,
                    Name = t.Coach.LastName + " " + t.Coach.FirstName,
                    DateOfBirth = t.Coach.DateOfBirth
                }
            });

            return View("~/Views/Teams/Details.cshtml", model);
        }

        public PartialViewResult PlayerList(int id)
        {
            var model = new PlayerListModel
            {
                Players = _unitOfWork.PlayerRepository.GetMany(p => p.TeamId == id, p => new PlayerListModel.Player
                {
                    Id = p.Id,
                    Name = p.LastName + " " + p.FirstName,
                    DateOfBirth = p.DateOfBirth,
                    FootballMatchCount = p.FootballMatchPlayers.Count(mp => mp.IsStarted || mp.EnterTime.HasValue)
                }).ToArray()
            };

            var playerIds = model.Players.Select(p => p.Id).ToArray();
            var playersGoals = _unitOfWork.FootballMatchPlayerGoalRepository.GetMany(g => playerIds.Contains(g.FootballMatchPlayer.PlayerId), g => new
            {
                g.FootballMatchPlayer.PlayerId
            }).ToArray();

            foreach (var player in model.Players)
            {
                player.FootballMatchGoalCount = playersGoals.Count(g => g.PlayerId == player.Id);
            }

            return PartialView("~/Views/Teams/Partials/PlayerList.cshtml", model);
        }

        public PartialViewResult MatchList(int id)
        {
            var model = new MatchListModel
            {
                FootballMatches = _unitOfWork.FootballMatchRepository.GetMany(m => m.FootballMatchTeams.Any(t => t.TeamId == id), m => new MatchListModel.FootballMatch
                {
                    Id = m.Id,
                    TournamentId = m.TournamentId,
                    TournamentName = m.Tournament.Name,
                    StageType = m.StageType,
                    Teams = m.FootballMatchTeams.Select(mt => new MatchListModel.FootballMatch.Team
                    {
                        Id = mt.TeamId,
                        Name = mt.Team.Name,
                        IsGuest = mt.IsGuest
                    })
                }).ToArray()
            };

            var footballMatchIds = model.FootballMatches.Select(m => m.Id).ToArray();
            var footballMatchesGoals = _unitOfWork.FootballMatchPlayerGoalRepository.GetMany(g => footballMatchIds.Contains(g.FootballMatchPlayer.FootballMatchId), g => new
            {
                g.FootballMatchPlayer.FootballMatchId,
                g.FootballMatchPlayer.Player.TeamId
            }).GroupBy(g => g.FootballMatchId).ToArray();

            foreach (var footballMatchGoals in footballMatchesGoals)
            {
                var footballMatch = model.FootballMatches.Single(m => m.Id == footballMatchGoals.Key);

                foreach (var team in footballMatch.Teams)
                {
                    team.Score = footballMatchGoals.Count(g => g.TeamId == team.Id);
                }
            }

            return PartialView("~/Views/Teams/Partials/MatchListModel.cshtml", model);
        }
    }
}