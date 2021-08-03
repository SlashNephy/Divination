using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Divination.ACT.TheHuntNotifier.TheHunt.Data;

namespace Divination.ACT.TheHuntNotifier.TheHunt
{
    public class TheHuntClient : IDisposable
    {
        private readonly HttpClient httpClient;

        public TheHuntClient()
        {
            httpClient = new HttpClient();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public async Task<ApiResponse> Fetch(World world)
        {
            var request =
                new HttpRequestMessage(HttpMethod.Get, $"https://ffxiv-the-hunt.net/api/data/2/world/{world}");
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
            request.Headers.Add("Referer",
                $"https://ffxiv-the-hunt.net/{Enum.GetName(typeof(World), world)?.ToLower()}");

            var response = await httpClient.SendAsync(request);

            var stream = await response.Content.ReadAsStreamAsync();
            return Parse(stream);
        }

        private ApiResponse Parse(Stream stream)
        {
            var entries = new List<ApiResponse.Entry>();

            using (var reader = new BigEndianBinaryReader(stream))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    var mobId = reader.ReadUInt32();
                    var reportsBytesLength = reader.ReadUInt32();

                    var reports = new List<Report>();
                    if (reportsBytesLength > 0)
                    {
                        var reportsBytes = reader.ReadBytes((int) reportsBytesLength);

                        var stream2 = new MemoryStream(reportsBytes);
                        using var reader2 = new BigEndianBinaryReader(stream2);
                        while (reader2.BaseStream.Position != reader2.BaseStream.Length)
                        {
                            try
                            {
                                var id = reader2.ReadBytes(16);
                                var uid = reader2.ReadBytes(16);
                                var instance = (int) reader2.ReadByte();
                                var time = (long) reader2.ReadUInt32();
                                var x = (int) reader2.ReadByte();
                                var y = (int) reader2.ReadByte();
                                var score = (int) reader2.ReadUInt16();

                                reports.Add(
                                    new Report(
                                        new Guid(id),
                                        new Guid(uid),
                                        instance,
                                        DateTimeOffset.FromUnixTimeSeconds(time).LocalDateTime,
                                        x != 0 && y != 0 ? new Point(x, y) : null,
                                        score
                                    )
                                );
                            }
                            catch (EndOfStreamException)
                            {
                                break;
                            }
                        }
                    }

                    entries.Add(
                        new ApiResponse.Entry((int) mobId, reports)
                    );
                }
            }

            return new ApiResponse(entries);
        }
    }
}
