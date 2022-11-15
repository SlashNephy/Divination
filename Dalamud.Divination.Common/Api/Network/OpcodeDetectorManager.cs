using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Game.Text;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Network
{
    public class OpcodeDetectorManager : INetworkHandler, IOpcodeDetectorManager
    {
        private readonly IChatClient chat;

        private readonly List<(IOpcodeDetector detector, uint step, Dictionary<string, ushort> definitions)> detectors =
            new();

        private readonly object detectorsLock = new();

        public OpcodeDetectorManager(IChatClient chat)
        {
            this.chat = chat;
        }

        public bool CanHandleReceivedMessage(NetworkContext context)
        {
            return true;
        }

        public void HandleReceivedMessage(NetworkContext context)
        {
            lock (detectorsLock)
            {
                foreach (var (index, (detector, step, definitions)) in detectors.Select((x, i) => (i, x)))
                {
                    if (detector.Detect(context, step, definitions))
                    {
                        var span = CollectionsMarshal.AsSpan(detectors);
                        ref var itemRef = ref span[index];
                        if (Unsafe.IsNullRef(ref itemRef))
                        {
                            continue;
                        }

                        itemRef.step++;
                        var description = detector.DescribeStep(itemRef.step);
                        if (description == default)
                        {
                            var result = new Dictionary<string, string>
                            {
                                {"Version", GameVersion.ReadCurrent().ToString()},
                                {"Patch", "???"},
                            };

                            foreach (var (key, value) in itemRef.definitions)
                            {
                                result[key] = $"0x{value:X4}";
                            }

                            chat.Print(JsonConvert.SerializeObject(result, Formatting.Indented));
                        }
                        else
                        {
                            chat.Print(description);
                        }
                    }
                }
            }
        }

        public void Register(IOpcodeDetector detector)
        {
            var description = detector.DescribeStep(0);
            if (description != default)
            {
                lock (detectorsLock)
                {
                    detectors.Add((detector, 0, new Dictionary<string, ushort>()));
                }
                chat.Print(description, type: XivChatType.Notice);
            }
        }
    }
}
