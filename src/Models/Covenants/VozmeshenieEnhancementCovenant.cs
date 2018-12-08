﻿using System.Collections.Generic;

namespace src.Models.Covenants
{
    public class VozmeshenieEnhancementCovenant : BaseCovenant
    {
        public VozmeshenieEnhancementCovenant()
        {
            CovenantName = CovenantType.VozmeshenieEnhancement;

            KeyWords = new List<string>
            {
                "Арендатор не имеет права на возмещение стоимости неотделимых улучшений Помещений",
                "возмещение",
                "неотделимые улучшения"
            };
        }
    }
}