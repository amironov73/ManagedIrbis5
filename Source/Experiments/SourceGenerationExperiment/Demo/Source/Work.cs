// ReSharper disable CheckNamespace

using ManagedIrbis;
using ManagedIrbis.Mapping;

namespace Demo;

internal partial class Work
{
    [FieldMapper]
    private partial Canary ConvertForward (Field from, Canary to);

    [FieldMapper]
    private partial Field ConvertBackward (Canary from, Field to);

    public void DoMapping()
    {
        var field = new Field
        {
            Subfields =
            {
                new SubField { Code = 'a', Value = "SubA" },
                new SubField { Code = 'b', Value = "SubB" },
                new SubField { Code = 'c', Value = "SubC" },
                new SubField { Code = 'd', Value = "SubD" },
                new SubField { Code = 'z', Value = "SubZ" },
            }
        };
        var canary = new Canary
        {
            Fourth = "Fourth"
        };
        var result1 = ConvertForward (field, canary);
        Console.WriteLine (result1);

        canary.First = "ASub";
        canary.Second = "BSub";
        canary.Third = "CSub";
        canary.Fourth = "DSub";
        var result2 = ConvertBackward (canary,
            new Field { Tag = 100, Subfields =
        {
            new SubField { Code = 'z', Value = "ZSub" }
        }});
        Console.WriteLine (result2);
    }
}
