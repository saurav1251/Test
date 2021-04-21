using Test.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Test.Services.Language
{
    public interface ILanguageTranslationService
    {
        List<LanguageTranslationResponse> GetLanguageTranslationByID(long loginId,string langId);

    }
}
