using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data.DesignTemplate
{
    internal class GEntityTypeGenerator : CSharpEntityTypeGenerator
    {
        public GEntityTypeGenerator(IAnnotationCodeGenerator annotationCodeGenerator, ICSharpHelper cSharpHelper) : base(annotationCodeGenerator, cSharpHelper) { }

        public override string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            try
            {
                string code = base.WriteCode(entityType, @namespace, useDataAnnotations);

                var oldString = "public partial class " + entityType.Name;
                var newString = "public partial class " + entityType.Name + " : EntityBase";

                return code.Replace(oldString, newString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
