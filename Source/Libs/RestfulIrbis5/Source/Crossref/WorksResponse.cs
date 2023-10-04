// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WorksResponse.cs -- ответ на запрос о публикациях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.Crossref;

/* Пример ответа

{
  "status": "ok",
  "message-type": "work-list",
  "message-version": "1.0.0",
  "message": {
    "facets": {},
    "total-results": 6880,
    "items": [
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              6,
              16
            ]
          ],
          "date-time": "2022-06-16T12:44:08Z",
          "timestamp": 1655383448417
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "license": [
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "tdm",
            "delay-in-days": 0,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "vor",
            "delay-in-days": 0,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "am",
            "delay-in-days": 0,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2022,
                  2,
                  1
                ]
              ],
              "date-time": "2022-02-01T00:00:00Z",
              "timestamp": 1643673600000
            },
            "content-version": "stm-asf",
            "delay-in-days": 0,
            "URL": "https:\/\/doi.org\/10.15223\/policy-001"
          }
        ],
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "chair": [
          {
            "name": "Friends of Josiah Carberry",
            "sequence": "additional",
            "affiliation": []
          }
        ],
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "abstract": "<jats:p>El tema característico de las obras de Stone es el puente entre cultura y sociedad. Se pueden encontrar varias narrativas sobre el defecto fatal y la dialéctica posterior de la clase semiótica.  Así, Debord usa el término “el paradigma subtextual del consenso” para denotar una paradoja cultural. El sujeto se interpola en un discurso neocultural que incluye la sexualidad como totalidad. Pero la crítica de Marx al nihilismo prepatriarquialista afirma que la conciencia es capaz de significación. El tema principal del modelo de discurso cultural de Dietrich[1] no es la construcción, sino la neoconstrucción. Por lo tanto, existe cualquier cantidad de narrativas relacionadas con el paradigma textual de la narrativa. La teoría cultural pretextual sugiere que el contexto debe provenir del inconsciente colectivo.<\/jats:p>",
        "DOI": "10.5555\/12345678es",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2022,
              6,
              15
            ]
          ],
          "date-time": "2022-06-15T19:48:51Z",
          "timestamp": 1655322531000
        },
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/something",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Hacia una teoría unificada de la metafísica de alta energía: teoría mentecato de cuerdas"
        ],
        "prefix": "10.5555",
        "volume": "5",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": false,
            "suffix": "Jr.",
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": [
              {
                "name": "Department of Psychoceramics, Brown University"
              }
            ]
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2022,
              6,
              15
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "original-title": [
          "Toward a Unified Theory of High-Energy Metaphysics: Silly String Theory"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2022,
              6,
              15
            ]
          ],
          "date-time": "2022-06-15T19:48:53Z",
          "timestamp": 1655322533000
        },
        "score": 31.403744,
        "resource": {
          "primary": {
            "URL": "https:\/\/sandbox.publicknowledgeproject.org\/index.php\/jpc\/article\/view\/519"
          }
        },
        "translator": [
          {
            "given": "Josias",
            "family": "Zarzavilla",
            "sequence": "additional",
            "affiliation": []
          }
        ],
        "issued": {
          "date-parts": [
            [
              2022,
              6,
              15
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/12345678es",
        "relation": {
          "is-translation-of": [
            {
              "id-type": "doi",
              "id": "10.5555\/12345678",
              "asserted-by": "subject"
            }
          ]
        },
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2022,
              6,
              15
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              4
            ]
          ],
          "date-time": "2022-04-04T11:58:26Z",
          "timestamp": 1649073506435
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  12,
                  29
                ]
              ],
              "date-time": "2012-12-29T00:00:00Z",
              "timestamp": 1356739200000
            },
            "DOI": "10.5555\/777766665555",
            "type": "correction",
            "label": "Correction"
          },
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  12,
                  29
                ]
              ],
              "date-time": "2012-12-29T00:00:00Z",
              "timestamp": 1356739200000
            },
            "DOI": "10.5555\/666655554444",
            "type": "clarification",
            "label": "Clarification"
          }
        ],
        "reference-count": 0,
        "publisher": "Society of Psychoceramics",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              12,
              29
            ]
          ]
        },
        "DOI": "10.5555\/3030303030x",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T10:13:38Z",
          "timestamp": 1329473618000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Correction and Clarifications"
        ],
        "prefix": "10.32013",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2012,
              12,
              29
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T18:55:17Z",
          "timestamp": 1564080917000
        },
        "score": 31.403744,
        "resource": {
          "primary": {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-3030303030x.html"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              12,
              29
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                12,
                29
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                12,
                29
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/3030303030x",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              12,
              29
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              4
            ]
          ],
          "date-time": "2022-04-04T05:32:32Z",
          "timestamp": 1649050352797
        },
        "publisher-location": "Boston",
        "edition-number": "3",
        "reference-count": 0,
        "publisher": "Crossref Publishing",
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "DOI": "10.32013\/92m.e-7",
        "type": "book-series",
        "created": {
          "date-parts": [
            [
              2021,
              8,
              30
            ]
          ],
          "date-time": "2021-08-30T18:37:30Z",
          "timestamp": 1630348650000
        },
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Awesome Book Series"
        ],
        "prefix": "10.32013",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": true,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "additional",
            "affiliation": [
              {
                "id": [
                  {
                    "id": "https:\/\/ror.org\/05gq02987",
                    "id-type": "ROR",
                    "asserted-by": "publisher"
                  }
                ]
              }
            ]
          }
        ],
        "member": "17333",
        "deposited": {
          "date-parts": [
            [
              2021,
              8,
              30
            ]
          ],
          "date-time": "2021-08-30T18:37:32Z",
          "timestamp": 1630348652000
        },
        "score": 31.403744,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org\/xml-samples"
          }
        },
        "issued": {
          "date-parts": [
            [
              null
            ]
          ]
        },
        "references-count": 0,
        "URL": "http:\/\/dx.doi.org\/10.32013\/92m.e-7",
        "ISSN": [
          "0738-6656"
        ],
        "issn-type": [
          {
            "value": "0738-6656",
            "type": "print"
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              4,
              21
            ]
          ],
          "date-time": "2023-04-21T05:24:19Z",
          "timestamp": 1682054659553
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "content-domain": {
          "domain": [
            "sandbox.publicknowledgeproject.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2011,
              10,
              11
            ]
          ]
        },
        "DOI": "10.5555\/987654321",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              3,
              14
            ]
          ],
          "date-time": "2012-03-14T09:26:23Z",
          "timestamp": 1331717183000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/something",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "The Impact of Interactive Epistemologies on Cryptography"
        ],
        "prefix": "10.5555",
        "volume": "8",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2011,
              10,
              11
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T13:27:57Z",
          "timestamp": 1681997277000
        },
        "score": 31.3928,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/6"
          }
        },
        "issued": {
          "date-parts": [
            [
              2011,
              10,
              11
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2011,
                10,
                11
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2011,
                10,
                11
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/987654321",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2011,
              10,
              11
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              1
            ]
          ],
          "date-time": "2022-04-01T10:58:13Z",
          "timestamp": 1648810693665
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "license": [
          {
            "start": {
              "date-parts": [
                [
                  2013,
                  2,
                  3
                ]
              ],
              "date-time": "2013-02-03T00:00:00Z",
              "timestamp": 1359849600000
            },
            "content-version": "unspecified",
            "delay-in-days": 0,
            "URL": "http:\/\/www.crossref.org\/license"
          },
          {
            "start": {
              "date-parts": [
                [
                  2014,
                  2,
                  3
                ]
              ],
              "date-time": "2014-02-03T00:00:00Z",
              "timestamp": 1391385600000
            },
            "content-version": "unspecified",
            "delay-in-days": 365,
            "URL": "http:\/\/creativecommons.org\/licenses\/by\/3.0\/deed.en_US"
          }
        ],
        "funder": [
          {
            "name": "National Science Foundation",
            "award": [
              "psychoceramics-1152342"
            ]
          },
          {
            "name": "Basic Energy Sciences, Office of Science, U.S. Department of Energy",
            "award": [
              "high-energy-metaphysics-SC0001091"
            ]
          }
        ],
        "content-domain": {
          "domain": [
            "annalsofpsychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": true
        },
        "short-container-title": [
          "Annals Psychoceramics B"
        ],
        "published-print": {
          "date-parts": [
            [
              2013,
              2,
              3
            ]
          ]
        },
        "DOI": "10.5555\/515151",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2013,
              4,
              2
            ]
          ],
          "date-time": "2013-04-02T10:33:26Z",
          "timestamp": 1364898806000
        },
        "page": "1-8",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "The Global State of Psychoceramics Research"
        ],
        "prefix": "10.5555",
        "volume": "2013",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": true,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "container-title": [
          "Annals of Psychoceramics B"
        ],
        "language": "en",
        "link": [
          {
            "URL": "http:\/\/annalsofpsychoceramics.labs.crossref.org\/fulltext\/10.5555\/515151.pdf",
            "content-type": "application\/pdf",
            "content-version": "vor",
            "intended-application": "text-mining"
          },
          {
            "URL": "http:\/\/annalsofpsychoceramics.labs.crossref.org\/fulltext\/10.5555\/515151.xml",
            "content-type": "application\/xml",
            "content-version": "vor",
            "intended-application": "text-mining"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2020,
              11,
              25
            ]
          ],
          "date-time": "2020-11-25T21:10:54Z",
          "timestamp": 1606338654000
        },
        "score": 31.392487,
        "resource": {
          "primary": {
            "URL": "http:\/\/annalsofpsychoceramics.labs.crossref.org\/abstract\/515151\/"
          }
        },
        "issued": {
          "date-parts": [
            [
              2013,
              2,
              3
            ]
          ]
        },
        "references-count": 0,
        "alternative-id": [
          "515151",
          "515151"
        ],
        "URL": "http:\/\/dx.doi.org\/10.5555\/515151",
        "ISSN": [
          "5555-5152",
          "5555-6167"
        ],
        "issn-type": [
          {
            "value": "5555-5152",
            "type": "print"
          },
          {
            "value": "5555-6167",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2013,
              2,
              3
            ]
          ]
        },
        "assertion": [
          {
            "value": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "URL": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "order": 0,
            "name": "orcid",
            "label": "ORCID",
            "group": {
              "name": "identifiers",
              "label": "Identifiers"
            }
          },
          {
            "value": "2013-02-01",
            "order": 0,
            "name": "received",
            "label": "Received",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2013-02-01",
            "order": 2,
            "name": "reviewed",
            "label": "Reviewed",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2013-02-02",
            "order": 1,
            "name": "accepted",
            "label": "Accepted",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2013-02-03",
            "order": 2,
            "name": "published",
            "label": "Published",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              3,
              30
            ]
          ],
          "date-time": "2022-03-30T23:46:21Z",
          "timestamp": 1648683981290
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "license": [
          {
            "start": {
              "date-parts": [
                [
                  2013,
                  2,
                  3
                ]
              ],
              "date-time": "2013-02-03T00:00:00Z",
              "timestamp": 1359849600000
            },
            "content-version": "unspecified",
            "delay-in-days": 0,
            "URL": "http:\/\/creativecommons.org\/licenses\/by\/3.0\/deed.en_US"
          }
        ],
        "content-domain": {
          "domain": [
            "annalsofpsychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Annals Psychoceramics B"
        ],
        "published-print": {
          "date-parts": [
            [
              2013,
              2,
              4
            ]
          ]
        },
        "DOI": "10.5555\/525252",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2013,
              7,
              19
            ]
          ],
          "date-time": "2013-07-19T09:27:31Z",
          "timestamp": 1374226051000
        },
        "page": "9-14",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 1,
        "title": [
          "Is Psychoceramics All It Is Cracked Up To Be?"
        ],
        "prefix": "10.5555",
        "volume": "2013",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": true,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "container-title": [
          "Annals of Psychoceramics B"
        ],
        "language": "en",
        "link": [
          {
            "URL": "http:\/\/annalsofpsychoceramics.labs.crossref.org\/fulltext\/10.5555\/525252",
            "content-type": "unspecified",
            "content-version": "vor",
            "intended-application": "text-mining"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2020,
              11,
              25
            ]
          ],
          "date-time": "2020-11-25T21:10:55Z",
          "timestamp": 1606338655000
        },
        "score": 31.392487,
        "resource": {
          "primary": {
            "URL": "http:\/\/www.labs.crossref.org\/documents\/prospect\/examples\/annals_of_psychoceramics_b_525252_3.xml"
          }
        },
        "issued": {
          "date-parts": [
            [
              2013,
              2,
              4
            ]
          ]
        },
        "references-count": 0,
        "alternative-id": [
          "525252",
          "525252"
        ],
        "URL": "http:\/\/dx.doi.org\/10.5555\/525252",
        "ISSN": [
          "5555-5152",
          "5555-6167"
        ],
        "issn-type": [
          {
            "value": "5555-5152",
            "type": "print"
          },
          {
            "value": "5555-6167",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2013,
              2,
              4
            ]
          ]
        },
        "assertion": [
          {
            "value": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "URL": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "order": 0,
            "name": "orcid",
            "label": "ORCID",
            "group": {
              "name": "identifiers",
              "label": "Identifiers"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              3
            ]
          ],
          "date-time": "2022-04-03T22:48:20Z",
          "timestamp": 1649026100650
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2009,
                  9,
                  14
                ]
              ],
              "date-time": "2009-09-14T00:00:00Z",
              "timestamp": 1252886400000
            },
            "DOI": "10.5555\/12345678",
            "type": "retraction",
            "label": "Retraction"
          }
        ],
        "reference-count": 0,
        "publisher": "Society of Psychoceramics",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": true
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2009,
              9,
              14
            ]
          ]
        },
        "DOI": "10.5555\/24242424x",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              15
            ]
          ],
          "date-time": "2012-02-15T06:06:16Z",
          "timestamp": 1329285976000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Retraction: Toward a Unified Theory of High-Energy Metaphysics: Silly String Theory"
        ],
        "prefix": "10.32013",
        "volume": "5",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2009,
              9,
              14
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T14:55:14Z",
          "timestamp": 1564066514000
        },
        "score": 31.392487,
        "resource": {
          "primary": {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-24242424x.html"
          }
        },
        "issued": {
          "date-parts": [
            [
              2009,
              9,
              14
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2008,
                8,
                13
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2008,
                8,
                14
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/24242424x",
        "relation": {
          "has-reply": [
            {
              "id-type": "doi",
              "id": "10.5555\/12345678",
              "asserted-by": "object"
            }
          ]
        },
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2009,
              9,
              14
            ]
          ]
        },
        "assertion": [
          {
            "value": "90%",
            "name": "remorse",
            "label": "Level of Remorse",
            "group": {
              "name": "publication_notes",
              "label": "Publication Notes"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              8,
              14
            ]
          ],
          "date-time": "2023-08-14T13:19:21Z",
          "timestamp": 1692019161005
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "DOI": "10.5555\/777766665555",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T10:13:20Z",
          "timestamp": 1329473600000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 1,
        "title": [
          "Deconstructing Write-Back Caches"
        ],
        "prefix": "10.5555",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T13:27:58Z",
          "timestamp": 1681997278000
        },
        "score": 31.392487,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/7"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/777766665555",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "assertion": [
          {
            "value": "Data are available at dryad digital repository:",
            "order": 0,
            "name": "related_data",
            "label": "Related Data",
            "group": {
              "name": "data_access",
              "label": "Data Access"
            }
          },
          {
            "value": "http:\/\/datadryad.org\/",
            "URL": "http:\/\/datadryad.org\/",
            "order": 1,
            "name": "related_data",
            "group": {
              "name": "data_access"
            }
          },
          {
            "value": "http:\/\/dx.doi.org\/10.5061\/dryad.90525",
            "URL": "http:\/\/dx.doi.org\/10.5061\/dryad.90525",
            "order": 2,
            "name": "related_data",
            "label": "Related Data",
            "group": {
              "name": "data_access",
              "label": "Data Access"
            }
          },
          {
            "value": "Data are available at dryad digital repository",
            "name": "related_data",
            "label": "Related Data",
            "explanation": {
              "URL": "http:\/\/dx.doi.org\/10.5061\/dryad.90525"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              9,
              14
            ]
          ],
          "date-time": "2023-09-14T00:11:28Z",
          "timestamp": 1694650288888
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "22",
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "JTD"
        ],
        "published-print": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "DOI": "10.5555\/bg4ed",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ],
          "date-time": "2020-06-02T08:44:57Z",
          "timestamp": 1591087497000
        },
        "page": "1-3",
        "source": "Crossref",
        "is-referenced-by-count": 1,
        "title": [
          "Worst Cat Ever"
        ],
        "prefix": "10.5555",
        "volume": "2020",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "container-title": [
          "Journal of Test Deposits"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ],
          "date-time": "2020-06-02T08:44:58Z",
          "timestamp": 1591087498000
        },
        "score": 31.392487,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org\/images\/labs\/worst_cat.png"
          }
        },
        "issued": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "22",
          "published-online": {
            "date-parts": [
              [
                2020,
                6,
                2
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2020,
                6,
                2
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/bg4ed",
        "ISSN": [
          "0198-8220"
        ],
        "issn-type": [
          {
            "value": "0198-8220",
            "type": "print"
          }
        ],
        "published": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              1,
              8
            ]
          ],
          "date-time": "2023-01-08T09:17:48Z",
          "timestamp": 1673169468850
        },
        "publisher-location": "Boston",
        "reference-count": 8,
        "publisher": "Crossref",
        "funder": [
          {
            "DOI": "10.13039\/100000001",
            "name": "National Science Foundation",
            "doi-asserted-by": "publisher",
            "award": [
              "CHE-1152342"
            ]
          }
        ],
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "accepted": {
          "date-parts": [
            [
              2018,
              10,
              20
            ]
          ]
        },
        "abstract": "<jats:p>The implications of ambimorphic archetypes have been far-reaching and pervasive. After years of natural research into consistent hashing, we argue the simulation of public-private key pairs, which embodies the confirmed principles of theory. Such a hypothesis might seem perverse but is derived from known results. Our focus in this paper is not on whether the well-known knowledge-based algorithm for the emulation of checksums by Herbert Simon runs in θ( n ) time, but rather on exploring a semantic tool for harnessing telephony. <\/jats:p>",
        "DOI": "10.32013\/aubagie",
        "type": "proceedings-article",
        "created": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T19:33:18Z",
          "timestamp": 1564083198000
        },
        "update-policy": "http:\/\/dx.doi.org\/10.32013\/hk7vasw",
        "source": "Crossref",
        "is-referenced-by-count": 4,
        "title": [
          "The importance of good examples"
        ],
        "prefix": "10.32013",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": true,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": [
              {
                "id": [
                  {
                    "id": "https:\/\/ror.org\/05bp8ka05",
                    "id-type": "ROR",
                    "asserted-by": "publisher"
                  }
                ]
              }
            ]
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "reference": [
          {
            "key": "ref1",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/r7py5MH"
          },
          {
            "key": "ref2",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/U8OyJ5X"
          },
          {
            "key": "ref3",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/nqGDdQr"
          },
          {
            "key": "ref4",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/o8VRnUO"
          },
          {
            "key": "ref5",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/En10keK"
          },
          {
            "key": "ref6",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/OAqGNhe"
          },
          {
            "key": "ref7",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/iMleOZB"
          },
          {
            "key": "ref8",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/kQZhpJi"
          }
        ],
        "event": {
          "name": "Crossref LIVE18",
          "theme": "How good is your metadata?",
          "location": "Toronto, ON",
          "acronym": "LIVE",
          "number": "18",
          "sponsor": [
            "Crossref Fan Club"
          ],
          "start": {
            "date-parts": [
              [
                2018,
                11,
                12
              ]
            ]
          },
          "end": {
            "date-parts": [
              [
                2018,
                11,
                14
              ]
            ]
          }
        },
        "container-title": [
          "Crossref LIVE Example Proceedings",
          "Crossref LIVE18"
        ],
        "original-title": [
          "La importancia de los buenos ejemplos"
        ],
        "link": [
          {
            "URL": "https:\/\/www.crossref.org\/example.xml",
            "content-type": "text\/xml",
            "content-version": "vor",
            "intended-application": "text-mining"
          },
          {
            "URL": "https:\/\/www.crossref.org\/faqs.html",
            "content-type": "unspecified",
            "content-version": "vor",
            "intended-application": "similarity-checking"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2021,
              9,
              14
            ]
          ],
          "date-time": "2021-09-14T17:34:11Z",
          "timestamp": 1631640851000
        },
        "score": 31.389498,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org\/xml-samples\/"
          }
        },
        "issued": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "references-count": 8,
        "URL": "http:\/\/dx.doi.org\/10.32013\/aubagie",
        "archive": [
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB",
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB",
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB"
        ],
        "ISSN": [
          "2169-2750"
        ],
        "issn-type": [
          {
            "value": "2169-2750",
            "type": "print"
          }
        ],
        "published": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "assertion": [
          {
            "value": "2012-07-24",
            "order": 0,
            "name": "received",
            "label": "Received",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-08-29",
            "order": 1,
            "name": "accepted",
            "label": "Accepted",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-09-10",
            "order": 2,
            "name": "published",
            "label": "Published",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              1
            ]
          ],
          "date-time": "2022-04-01T19:56:24Z",
          "timestamp": 1648842984643
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  2,
                  29
                ]
              ],
              "date-time": "2012-02-29T00:00:00Z",
              "timestamp": 1330473600000
            },
            "DOI": "10.5555\/12345679",
            "type": "correction",
            "label": "Correction"
          }
        ],
        "reference-count": 0,
        "publisher": "Society of Psychoceramics",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              2,
              29
            ]
          ]
        },
        "DOI": "10.5555\/25252525x",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              16
            ]
          ],
          "date-time": "2012-02-16T14:26:14Z",
          "timestamp": 1329402374000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Correction: Developing Thin Clients Using Amphibious Epistemologies"
        ],
        "prefix": "10.32013",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2012,
              2,
              29
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T18:55:16Z",
          "timestamp": 1564080916000
        },
        "score": 31.389498,
        "resource": {
          "primary": {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-25252525x.html"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              2,
              29
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                2,
                29
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                2,
                29
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/25252525x",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              2,
              29
            ]
          ]
        },
        "assertion": [
          {
            "value": "9 (out of 10)",
            "name": "scale_of_mistake",
            "label": "Scale of Mistake",
            "group": {
              "name": "publication_notes",
              "label": "Publication Notes"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              4,
              21
            ]
          ],
          "date-time": "2023-04-21T05:24:16Z",
          "timestamp": 1682054656617
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  5,
                  12
                ]
              ],
              "date-time": "2012-05-12T00:00:00Z",
              "timestamp": 1336780800000
            },
            "DOI": "10.5555\/12345681",
            "type": "correction",
            "label": "Correction"
          }
        ],
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              5,
              6
            ]
          ]
        },
        "DOI": "10.5555\/12345681",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T09:32:04Z",
          "timestamp": 1329471124000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Dog: A Methodology for the Development of Simulated Annealing"
        ],
        "prefix": "10.5555",
        "volume": "5",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2012,
              5,
              6
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T13:27:58Z",
          "timestamp": 1681997278000
        },
        "score": 31.389498,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/5"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              5,
              6
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                5,
                6
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                5,
                6
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/12345681",
        "relation": {
          "is-based-on": [
            {
              "id-type": "doi",
              "id": "10.5284\/1000389",
              "asserted-by": "subject"
            }
          ],
          "has-review": [
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9882",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9881",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9884",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9883",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9880",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9879",
              "asserted-by": "object"
            },
            {
              "id-type": "doi",
              "id": "10.5555\/12345681.9885",
              "asserted-by": "object"
            }
          ]
        },
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              5,
              6
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              4,
              21
            ]
          ],
          "date-time": "2023-04-21T05:37:01Z",
          "timestamp": 1682055421220
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2018,
                  1,
                  1
                ]
              ],
              "date-time": "2018-01-01T00:00:00Z",
              "timestamp": 1514764800000
            },
            "DOI": "10.5555\/12345678",
            "type": "corrigendum",
            "label": "Corrigendum"
          }
        ],
        "reference-count": 1,
        "publisher": "Test accounts",
        "issue": "11",
        "license": [
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "tdm",
            "delay-in-days": 1195,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "vor",
            "delay-in-days": 1195,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2011,
                  11,
                  21
                ]
              ],
              "date-time": "2011-11-21T00:00:00Z",
              "timestamp": 1321833600000
            },
            "content-version": "am",
            "delay-in-days": 1195,
            "URL": "http:\/\/psychoceramicsproprietrylicenseV1.com"
          },
          {
            "start": {
              "date-parts": [
                [
                  2022,
                  2,
                  1
                ]
              ],
              "date-time": "2022-02-01T00:00:00Z",
              "timestamp": 1643673600000
            },
            "content-version": "stm-asf",
            "delay-in-days": 4920,
            "URL": "https:\/\/doi.org\/10.15223\/policy-001"
          }
        ],
        "funder": [
          {
            "DOI": "10.13039\/100000001",
            "name": "National Science Foundation",
            "doi-asserted-by": "publisher",
            "award": [
              "12345678"
            ]
          },
          {
            "DOI": "10.13039\/100006151",
            "name": "Basic Energy Sciences, Office of Science, U.S. Department of Energy",
            "doi-asserted-by": "publisher",
            "award": [
              "12345679"
            ]
          }
        ],
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "chair": [
          {
            "name": "Friends of Josiah Carberry",
            "sequence": "additional",
            "affiliation": []
          }
        ],
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2008,
              8,
              14
            ]
          ]
        },
        "abstract": "<jats:p>The characteristic theme of the works of Stone is the bridge between culture and society. Several narratives concerning the fatal !aw, and subsequent dialectic, of semioticist class may be found. Thus, Debord uses the term ‘the subtextual paradigm of consensus’ to denote a cultural paradox. The subject is interpolated into a neocultural discourse that includes sexuality as a totality. But Marx’s critique of prepatriarchialist nihilism states that consciousness is capable of signi\"cance. The main theme of Dietrich’s[1]model of cultural discourse is not construction, but neoconstruction. Thus, any number of narratives concerning the textual paradigm of narrative exist. Pretextual cultural theory suggests that context must come from the collective unconscious.<\/jats:p>",
        "DOI": "10.5555\/12345678",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2011,
              11,
              9
            ]
          ],
          "date-time": "2011-11-09T14:42:05Z",
          "timestamp": 1320849725000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/something",
        "source": "Crossref",
        "is-referenced-by-count": 3,
        "title": [
          "Toward a Unified Theory of High-Energy Metaphysics: Silly String Theory"
        ],
        "prefix": "10.5555",
        "volume": "5",
        "clinical-trial-number": [
          {
            "clinical-trial-number": "isrctn12345",
            "registry": "10.18810\/isrctn"
          }
        ],
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": false,
            "suffix": "Jr.",
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": [
              {
                "name": "Department of Psychoceramics, Brown University"
              }
            ]
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2008,
              8,
              13
            ]
          ]
        },
        "reference": [
          {
            "key": "ref0",
            "doi-asserted-by": "publisher",
            "DOI": "10.37717\/220020589"
          }
        ],
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "link": [
          {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-12345678.html",
            "content-type": "unspecified",
            "content-version": "vor",
            "intended-application": "similarity-checking"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T12:29:52Z",
          "timestamp": 1681993792000
        },
        "score": 31.389498,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/2"
          }
        },
        "issued": {
          "date-parts": [
            [
              2008,
              8,
              13
            ]
          ]
        },
        "references-count": 1,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/12345678",
        "archive": [
          "CLOCKSS"
        ],
        "relation": {
          "is-reply-to": [
            {
              "id-type": "doi",
              "id": "10.5555\/24242424x",
              "asserted-by": "subject"
            }
          ],
          "references": [
            {
              "id-type": "doi",
              "id": "10.5284\/1000389",
              "asserted-by": "object"
            }
          ],
          "has-translation": [
            {
              "id-type": "doi",
              "id": "10.5555\/12345678es",
              "asserted-by": "object"
            }
          ]
        },
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2008,
              8,
              13
            ]
          ]
        },
        "assertion": [
          {
            "URL": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "order": 0,
            "name": "orcid",
            "label": "ORCID",
            "explanation": {
              "URL": "IDs for Or"
            }
          },
          {
            "value": "2012-07-24",
            "order": 0,
            "name": "received",
            "label": "Received",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-08-29",
            "order": 1,
            "name": "accepted",
            "label": "Accepted",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-09-26",
            "order": 2,
            "name": "published_online",
            "label": "Published Online",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-10-27",
            "order": 3,
            "name": "published_print",
            "label": "Published Print",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-12345678.html",
            "order": 0,
            "name": "peer_reviewed",
            "label": "Peer reviewed",
            "explanation": {
              "URL": "thrice"
            },
            "group": {
              "name": "peer_review",
              "label": "Peer review"
            }
          },
          {
            "URL": "http:\/\/www.silly-string.com\/silly-info\/index.cfm",
            "order": 0,
            "name": "supplementary_Material",
            "label": "Supplementary Material",
            "explanation": {
              "URL": "Helpful Silly String resource"
            }
          },
          {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-12345678.html",
            "order": 0,
            "name": "licensing",
            "label": "Licensing Information",
            "explanation": {
              "URL": "Complicated license information available"
            },
            "group": {
              "name": "copyright_licensing",
              "label": "Copyright & Licensing"
            }
          },
          {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-12345678.html",
            "order": 1,
            "name": "copyright_statement",
            "label": "Copyright Statement",
            "explanation": {
              "URL": "Lorem Copyrightsum"
            },
            "group": {
              "name": "copyright_licensing",
              "label": "Copyright & Licensing"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              8,
              14
            ]
          ],
          "date-time": "2023-08-14T13:16:17Z",
          "timestamp": 1692018977924
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "license": [
          {
            "start": {
              "date-parts": [
                [
                  2008,
                  2,
                  29
                ]
              ],
              "date-time": "2008-02-29T00:00:00Z",
              "timestamp": 1204243200000
            },
            "content-version": "tdm",
            "delay-in-days": 0,
            "URL": "http:\/\/creativecommons.org\/licenses\/by-nc\/3.0"
          }
        ],
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2008,
              2,
              29
            ]
          ]
        },
        "DOI": "10.5555\/12345679",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              16
            ]
          ],
          "date-time": "2012-02-16T13:57:13Z",
          "timestamp": 1329400633000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 1,
        "title": [
          "Developing Thin Clients Using Amphibious Epistemologies"
        ],
        "prefix": "10.5555",
        "volume": "5",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2008,
              2,
              29
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "link": [
          {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/site\/example2\/article_b.pdf",
            "content-type": "unspecified",
            "content-version": "tdm",
            "intended-application": "text-mining"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T13:27:56Z",
          "timestamp": 1681997276000
        },
        "score": 31.389498,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/3"
          }
        },
        "issued": {
          "date-parts": [
            [
              2008,
              2,
              29
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2008,
                2,
                29
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/12345679",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2008,
              2,
              29
            ]
          ]
        },
        "assertion": [
          {
            "value": "Data are available at dryad digital repository (<uri xlink:href=\"http:\/\/datadryad.org\/\">http:\/\/datadryad.org\/<\/uri>): <ext-link ext-link-type=\"uri\" xlink:href=\"http:\/\/dx.doi.org\/10.5061\/dryad.90525\">doi:10.5061\/dryad.90525<\/ext-link>",
            "name": "related-data",
            "label": "Related Data",
            "group": {
              "name": "data_access",
              "label": "Data accessibility"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              2
            ]
          ],
          "date-time": "2022-04-02T04:52:10Z",
          "timestamp": 1648875130585
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "119",
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "JRT"
        ],
        "published-print": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "abstract": "<jats:p>hi again<\/jats:p>",
        "DOI": "10.5555\/zippo",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ],
          "date-time": "2020-06-02T10:30:24Z",
          "timestamp": 1591093824000
        },
        "page": "1-4",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Bad Catz"
        ],
        "prefix": "10.5555",
        "volume": "42",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": false,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "container-title": [
          "Journal of Test Deposits"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2021,
              4,
              14
            ]
          ],
          "date-time": "2021-04-14T19:29:12Z",
          "timestamp": 1618428552000
        },
        "score": 31.271557,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org"
          }
        },
        "issued": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "119",
          "published-print": {
            "date-parts": [
              [
                2012,
                9,
                1
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/zippo",
        "ISSN": [
          "0198-8220"
        ],
        "issn-type": [
          {
            "value": "0198-8220",
            "type": "print"
          }
        ],
        "published": {
          "date-parts": [
            [
              2020,
              6,
              2
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              5
            ]
          ],
          "date-time": "2022-04-05T16:11:20Z",
          "timestamp": 1649175080437
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  2,
                  29
                ]
              ],
              "date-time": "2012-02-29T00:00:00Z",
              "timestamp": 1330473600000
            },
            "DOI": "10.5555\/987654321",
            "type": "correction",
            "label": "Correction"
          }
        ],
        "reference-count": 0,
        "publisher": "Society of Psychoceramics",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "DOI": "10.5555\/29292929x",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T09:52:12Z",
          "timestamp": 1329472332000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Correction: The Impact of Interactive Epistemologies on Cryptography"
        ],
        "prefix": "10.32013",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T18:55:16Z",
          "timestamp": 1564080916000
        },
        "score": 31.271557,
        "resource": {
          "primary": {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-29292929x.html"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/29292929x",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              4
            ]
          ],
          "date-time": "2022-04-04T05:04:12Z",
          "timestamp": 1649048652489
        },
        "update-to": [
          {
            "updated": {
              "date-parts": [
                [
                  2012,
                  3,
                  15
                ]
              ],
              "date-time": "2012-03-15T00:00:00Z",
              "timestamp": 1331769600000
            },
            "DOI": "10.5555\/12345680",
            "type": "interesting_update",
            "label": "Interesting Update"
          }
        ],
        "reference-count": 0,
        "publisher": "Society of Psychoceramics",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              3,
              15
            ]
          ]
        },
        "DOI": "10.5555\/26262626x",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T03:44:52Z",
          "timestamp": 1329450292000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "Interesting Update to A Methodology for the Emulation of Architecture"
        ],
        "prefix": "10.32013",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2012,
              3,
              15
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2019,
              7,
              25
            ]
          ],
          "date-time": "2019-07-25T14:55:16Z",
          "timestamp": 1564066516000
        },
        "score": 31.271557,
        "resource": {
          "primary": {
            "URL": "http:\/\/psychoceramics.labs.crossref.org\/10.5555-26262626x.html"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              3,
              15
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                3,
                15
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                3,
                15
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/26262626x",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              3,
              15
            ]
          ]
        },
        "assertion": [
          {
            "value": "Mega-Interesting",
            "name": "level_of_interestingness",
            "label": "Interestingness",
            "group": {
              "name": "publication_notes",
              "label": "Publication Notes"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              4,
              4
            ]
          ],
          "date-time": "2022-04-04T00:05:27Z",
          "timestamp": 1649030727643
        },
        "publisher-location": "Boston",
        "reference-count": 8,
        "publisher": "Crossref",
        "funder": [
          {
            "DOI": "10.13039\/100000001",
            "name": "National Science Foundation",
            "doi-asserted-by": "publisher",
            "award": [
              "CHE-1152342"
            ]
          }
        ],
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "accepted": {
          "date-parts": [
            [
              2018,
              10,
              20
            ]
          ]
        },
        "abstract": "<jats:p>The implications of ambimorphic archetypes have been far-reaching and pervasive. After years of natural research into consistent hashing, we argue the simulation of public-private key pairs, which embodies the confirmed principles of theory. Such a hypothesis might seem perverse but is derived from known results. Our focus in this paper is not on whether the well-known knowledge-based algorithm for the emulation of checksums by Herbert Simon runs in θ( n ) time, but rather on exploring a semantic tool for harnessing telephony. <\/jats:p>",
        "DOI": "10.32013\/aubxxagie",
        "type": "proceedings-article",
        "created": {
          "date-parts": [
            [
              2021,
              9,
              16
            ]
          ],
          "date-time": "2021-09-16T14:28:56Z",
          "timestamp": 1631802536000
        },
        "update-policy": "http:\/\/dx.doi.org\/10.32013\/hk7vasw",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "The importance of good examples"
        ],
        "prefix": "10.32013",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": true,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": [
              {
                "name": "Brown University"
              }
            ]
          }
        ],
        "member": "17333",
        "published-online": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "reference": [
          {
            "key": "ref1",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/r7py5MH"
          },
          {
            "key": "ref2",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/U8OyJ5X"
          },
          {
            "key": "ref3",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/nqGDdQr"
          },
          {
            "key": "ref4",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/o8VRnUO"
          },
          {
            "key": "ref5",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/En10keK"
          },
          {
            "key": "ref6",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/OAqGNhe"
          },
          {
            "key": "ref7",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/iMleOZB"
          },
          {
            "key": "ref8",
            "doi-asserted-by": "publisher",
            "DOI": "10.32013\/kQZhpJi"
          }
        ],
        "event": {
          "name": "Crossref LIVE18",
          "theme": "How good is your metadata?",
          "location": "Toronto, ON",
          "acronym": "LIVE",
          "number": "18",
          "sponsor": [
            "Crossref Fan Club"
          ],
          "start": {
            "date-parts": [
              [
                2018,
                11,
                12
              ]
            ]
          },
          "end": {
            "date-parts": [
              [
                2018,
                11,
                14
              ]
            ]
          }
        },
        "container-title": [
          "Crossref LIVE Example Proceedings",
          "Crossref LIVE18"
        ],
        "original-title": [
          "La importancia de los buenos ejemplos"
        ],
        "link": [
          {
            "URL": "https:\/\/www.crossref.org\/example.xml",
            "content-type": "text\/xml",
            "content-version": "vor",
            "intended-application": "text-mining"
          },
          {
            "URL": "https:\/\/www.crossref.org\/faqs.html",
            "content-type": "unspecified",
            "content-version": "vor",
            "intended-application": "similarity-checking"
          }
        ],
        "deposited": {
          "date-parts": [
            [
              2021,
              9,
              16
            ]
          ],
          "date-time": "2021-09-16T14:28:57Z",
          "timestamp": 1631802537000
        },
        "score": 31.271557,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org\/xml-samples\/"
          }
        },
        "issued": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "references-count": 8,
        "URL": "http:\/\/dx.doi.org\/10.32013\/aubxxagie",
        "archive": [
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB",
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB",
          "CLOCKSS",
          "Internet Archive",
          "Portico",
          "KB"
        ],
        "ISSN": [
          "2169-2750"
        ],
        "issn-type": [
          {
            "value": "2169-2750",
            "type": "print"
          }
        ],
        "published": {
          "date-parts": [
            [
              2018,
              11,
              23
            ]
          ]
        },
        "assertion": [
          {
            "value": "2012-07-24",
            "order": 0,
            "name": "received",
            "label": "Received",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-08-29",
            "order": 1,
            "name": "accepted",
            "label": "Accepted",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          },
          {
            "value": "2012-09-10",
            "order": 2,
            "name": "published",
            "label": "Published",
            "group": {
              "name": "publication_history",
              "label": "Publication History"
            }
          }
        ]
      },
      {
        "indexed": {
          "date-parts": [
            [
              2023,
              8,
              14
            ]
          ],
          "date-time": "2023-08-14T13:15:46Z",
          "timestamp": 1692018946704
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "content-domain": {
          "domain": [
            "psychoceramics.labs.crossref.org"
          ],
          "crossmark-restriction": false
        },
        "short-container-title": [
          "Journal of Psychoceramics"
        ],
        "published-print": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "DOI": "10.5555\/666655554444",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2012,
              2,
              17
            ]
          ],
          "date-time": "2012-02-17T10:13:23Z",
          "timestamp": 1329473603000
        },
        "page": "1-3",
        "update-policy": "http:\/\/dx.doi.org\/10.5555\/crossmark_policy",
        "source": "Crossref",
        "is-referenced-by-count": 1,
        "title": [
          "The Memory Bus Considered Harmful"
        ],
        "prefix": "10.5555",
        "volume": "9",
        "author": [
          {
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "published-online": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2023,
              4,
              20
            ]
          ],
          "date-time": "2023-04-20T13:27:56Z",
          "timestamp": 1681997276000
        },
        "score": 31.271557,
        "resource": {
          "primary": {
            "URL": "https:\/\/ojs33.crossref.publicknowledgeproject.org\/index.php\/test\/article\/view\/8"
          }
        },
        "issued": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-online": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          },
          "published-print": {
            "date-parts": [
              [
                2012,
                10,
                11
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.5555\/666655554444",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "electronic"
          }
        ],
        "published": {
          "date-parts": [
            [
              2012,
              10,
              11
            ]
          ]
        }
      },
      {
        "indexed": {
          "date-parts": [
            [
              2022,
              3,
              30
            ]
          ],
          "date-time": "2022-03-30T19:32:55Z",
          "timestamp": 1648668775966
        },
        "reference-count": 0,
        "publisher": "Test accounts",
        "issue": "11",
        "content-domain": {
          "domain": [],
          "crossmark-restriction": false
        },
        "published-print": {
          "date-parts": [
            [
              2019
            ]
          ]
        },
        "DOI": "10.32013\/pending-publication-minimum",
        "type": "journal-article",
        "created": {
          "date-parts": [
            [
              2019,
              10,
              28
            ]
          ],
          "date-time": "2019-10-28T14:52:15Z",
          "timestamp": 1572274335000
        },
        "page": "1",
        "source": "Crossref",
        "is-referenced-by-count": 0,
        "title": [
          "This is a test of the most basic Pending Publication metadata required"
        ],
        "prefix": "10.5555",
        "volume": "10",
        "author": [
          {
            "ORCID": "http:\/\/orcid.org\/0000-0002-1825-0097",
            "authenticated-orcid": false,
            "given": "Josiah",
            "family": "Carberry",
            "sequence": "first",
            "affiliation": []
          }
        ],
        "member": "7822",
        "container-title": [
          "Journal of Psychoceramics"
        ],
        "language": "en",
        "deposited": {
          "date-parts": [
            [
              2021,
              11,
              5
            ]
          ],
          "date-time": "2021-11-05T15:20:26Z",
          "timestamp": 1636125626000
        },
        "score": 31.21792,
        "resource": {
          "primary": {
            "URL": "https:\/\/www.crossref.org\/help\/pending-publication\/"
          }
        },
        "editor": [
          {
            "given": "Matt",
            "family": "Techthespian",
            "sequence": "additional",
            "affiliation": [
              {
                "name": "New York Institute of Technology"
              }
            ]
          }
        ],
        "issued": {
          "date-parts": [
            [
              2019
            ]
          ]
        },
        "references-count": 0,
        "journal-issue": {
          "issue": "11",
          "published-print": {
            "date-parts": [
              [
                2019
              ]
            ]
          }
        },
        "URL": "http:\/\/dx.doi.org\/10.32013\/pending-publication-minimum",
        "ISSN": [
          "0264-3561"
        ],
        "issn-type": [
          {
            "value": "0264-3561",
            "type": "print"
          }
        ],
        "published": {
          "date-parts": [
            [
              2019
            ]
          ]
        }
      }
    ],
    "items-per-page": 20,
    "query": {
      "start-index": 0,
      "search-terms": null
    }
  }
}

 */

/// <summary>
/// Ответ на запрос о публикациях.
/// </summary>
[PublicAPI]
public sealed class WorksResponse
    : MessageBase
{
    #region Properties

    /// <summary>
    /// Массив публикаций.
    /// </summary>
    [JsonProperty ("items")]
    public WorkItem[]? Items { get; set; }

    #endregion
}
