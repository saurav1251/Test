using Test.Data.SqlHelper;
using Test.Entities;
using Test.Services.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Test.Services.Language
{
    public class LanguageTranslationService : ILanguageTranslationService
    {
        private readonly IStoredProcBuilder _storedProcBuilder;

        public LanguageTranslationService(IStoredProcBuilder storedProcBuilder)
        {
            _storedProcBuilder = storedProcBuilder;
        }

        public List<LanguageTranslationResponse> GetLanguageTranslationByID(long loginId,string langId)
        {
            List<LanguageTranslationResponse> _response = null;
            _storedProcBuilder.StoredProc(StoredProcConstant.PROC_GetPreferredLanguage)
                .ClearParam()
                .AddParam("@i_prefLanguage", langId)
                .AddParam("@i_Mode", "Preferrence_Details")
                .Exec(r => _response = r.ToList<LanguageTranslationResponse>());

            return _response;
        }
    }
}
