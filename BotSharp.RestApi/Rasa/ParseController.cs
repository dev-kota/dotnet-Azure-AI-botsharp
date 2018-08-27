﻿using BotSharp.Core.Engines;
using BotSharp.Core.Engines.Rasa;
using BotSharp.Core.Models;
using BotSharp.NLP;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BotSharp.RestApi.Rasa
{
#if RASA_UI
    /// <summary>
    /// send a text request
    /// </summary>
    [Route("[controller]")]
    public class ParseController : ControllerBase
    {
        private readonly IBotPlatform _platform;

        /// <summary>
        /// Initialize dialog controller and get a platform instance
        /// </summary>
        /// <param name="platform"></param>
        public ParseController(IBotPlatform platform)
        {
            _platform = platform;
        }

        /// <summary>
        /// parse request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public ActionResult<RasaResponse> Parse()
        {
            var config = new AIConfiguration("", SupportedLanguage.English);
            config.SessionId = "rasa nlu";

            string body = "";
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }
            var request = JsonConvert.DeserializeObject<RasaRequestModel>(body);

            // Load agent
            var projectPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataPath").ToString(), "Projects", request.Project);
            var modelPath = Path.Combine(projectPath, request.Model);

            _platform.LoadAgentFromFile<AgentImporterInRasa>(modelPath);

            var aIResponse = _platform.TextRequest(new AIRequest
            {
                Model = request.Model,
                Query = new String[] { request.Text }
            });

            var rasaResponse = new RasaResponse
            {
                Intent = new RasaResponseIntent
                {
                    Name = aIResponse.Result.Metadata.IntentName,
                    Confidence = aIResponse.Result.Score
                },
                Entities = aIResponse.Result.Entities.Select(x => new RasaResponseEntity
                {
                    Extractor = x.Extrator,
                    Start = x.Start,
                    Entity = x.Entity,
                    Value = x.Value
                }).ToList(),
                Text = request.Text,
                Model = request.Model,
                Project = request.Project,
                IntentRanking = new List<RasaResponseIntent> { }
            };

            return rasaResponse;
        }
    }
#endif
}
