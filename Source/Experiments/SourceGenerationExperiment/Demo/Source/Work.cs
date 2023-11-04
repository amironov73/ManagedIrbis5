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

    [RecordMapper]
    private partial Raven ConvertForward (Record from, Raven to);

    [RecordMapper]
    private partial Record ConvertBackward (Raven from, Record to);

    public void DoMapping()
    {
        var field = new Field
        {
            Subfields =
            {
                new SubField { Code = 'a', Value = "SubA" },
                new SubField { Code = 'b', Value = "321" },
                new SubField { Code = 'c', Value = "True" },
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

        canary.First = null!;
        canary.Second = 123;
        canary.Third = false;
        canary.Fourth = "DSub";
        var result2 = ConvertBackward (canary,
            new Field { Tag = 100, Subfields =
        {
            new SubField { Code = 'z', Value = "ZSub" }
        }});
        Console.WriteLine (result2);

        var record = new Record
        {
            Fields =
            {
                new Field
                {
                    Tag = 100,
                    Subfields =
                    {
                        new SubField
                        {
                            Code = 'a',
                            Value = "100-a"
                        },
                        new SubField
                        {
                            Code = 'b',
                            Value = "100-b"
                        },
                    }
                },

                new Field
                {
                    Tag = 200,
                    Subfields =
                    {
                        new SubField
                        {
                            Code = 'a',
                            Value = "200-a"
                        },
                        new SubField
                        {
                            Code = 'b',
                            Value = "200-b"
                        },
                        new SubField
                        {
                            Code = 'c',
                            Value = "200-c"
                        },
                    }
                },

                new Field
                {
                    Tag = 300,
                    Subfields =
                    {
                        new SubField
                        {
                            Code = 'a',
                            Value = "300-a"
                        },
                        new SubField
                        {
                            Code = 'b',
                            Value = "300-b"
                        },
                        new SubField
                        {
                            Code = 'c',
                            Value = "300-c"
                        },
                        new SubField
                        {
                            Code = 'd',
                            Value = "300-d"
                        },
                    }
                },
            }
        };
        var raven = new Raven
        {
            Fourth = "Fourth"
        };
        var result3 = ConvertForward (record, raven);
        Console.WriteLine (result3);

        raven.First = "ASub";
        raven.Second = null;
        raven.Third = "CSub";
        raven.Fourth = "DSub";
        var result4 = ConvertBackward (raven,
            new Record
            {
                Fields =
                {
                    new Field { Tag = 600, Subfields =
                    {
                        new SubField { Code = 'z', Value = "600-z" }
                    }}
                }
            });
        Console.WriteLine ("********");
        Console.Write (result4);
        Console.WriteLine ("********");
    }
}
