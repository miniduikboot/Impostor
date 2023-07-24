using System.Collections.Generic;
using Impostor.Api.Games;
using Impostor.Api.Innersloth;
using Impostor.Api.Net.Manager;
using Microsoft.Extensions.Logging;

namespace Impostor.Server.Net.Manager
{
    internal class CompatibilityManager : ICompatibilityManager
    {
        private static readonly CompatData[] DefaultSupportedVersions =
        {
            new(GameVersion.GetVersion(2022, 11, 1)), // 2022.12.8

            new(GameVersion.GetVersion(2022, 11, 9)), // 2022.12.14

            new(GameVersion.GetVersion(2022, 12, 2)), // 2023.2.28

            new(
                GameVersion.GetVersion(2023, 4, 21), // 2023.6.13
                new[]
                {
                    GameVersion.GetVersion(2023, 1, 11), // 2023.3.28a
                    GameVersion.GetVersion(2023, 3, 13), // 2023.3.28
                }
            ),

            new(GameVersion.GetVersion(2023, 5, 20)), // 2023.7.11

            new(
                GameVersion.GetVersion(2222, 0, 0),
                new[]
                {
                    GameVersion.GetVersion(2023, 5, 20),
                },
                false
            ), // host-only mods
        };

        // Map from client version to compatibility group
        private readonly Dictionary<int, int[]?> _supportMap = new Dictionary<int, int[]?>();
        private readonly ILogger<CompatibilityManager> _logger;
        private readonly bool _initializationFinished;
        private int _lowestVersionSupported = int.MaxValue;
        private int _highestVersionSupported;

        public CompatibilityManager(ILogger<CompatibilityManager> logger)
        {
            _logger = logger;
            _initializationFinished = false;
            foreach (var compatData in DefaultSupportedVersions)
            {
                AddSupportedVersion(compatData.GameVersion, compatData.CompatGroup, compatData.IncludeInSupportRange);
            }

            ModifiedByUser = false;
            _initializationFinished = true;
        }

        internal bool ModifiedByUser { get; private set; }

        public ICompatibilityManager.VersionCompareResult TryGetCompatibilityGroup(int clientVersion)
        {
            if (_supportMap.ContainsKey(clientVersion))
            {
                return ICompatibilityManager.VersionCompareResult.Compatible;
            }
            else if (clientVersion < _lowestVersionSupported)
            {
                return ICompatibilityManager.VersionCompareResult.ClientTooOld;
            }
            else if (clientVersion > _highestVersionSupported)
            {
                return ICompatibilityManager.VersionCompareResult.ServerTooOld;
            }
            else
            {
                return ICompatibilityManager.VersionCompareResult.Unknown;
            }
        }

        public bool GetGameJoinError(int hostVersion, int clinetVersion, out GameJoinError error)
        {
            if (_supportMap.TryGetValue(hostVersion, out var compat) && compat != null)
            {
                foreach (var suppertVer in compat)
                {
                    if (suppertVer == clinetVersion)
                    {
                        error = GameJoinError.None;
                        return false;
                    }
                }
            }

            error = clinetVersion < hostVersion ? GameJoinError.ClientOutdated : GameJoinError.ClientTooNew;

            return true;
        }

        public void AddSupportedVersion(int gameVersion, int[]? compatGroup, bool includeInSupportRange)
        {
            if (_initializationFinished)
            {
                ModifiedByUser = true;
                _logger.LogWarning("AddSupportedVersion was called by a plugin, this can create unexpected issues. Please proceed carefully");
            }

            var compats = new List<int> { gameVersion };
            if (compatGroup != null)
            {
                foreach (var ver2 in compatGroup)
                {
                    compats.Add(ver2);
                }
            }

            foreach (var key in compats)
            {
                _supportMap[key] = compats.ToArray();
                if (includeInSupportRange)
                {
                    if (key < _lowestVersionSupported)
                    {
                        _lowestVersionSupported = key;
                    }

                    if (key > _highestVersionSupported)
                    {
                        _highestVersionSupported = key;
                    }
                }
            }
        }

        public bool RemoveSupportedVersion(int removedVersion)
        {
            return _supportMap.Remove(removedVersion);
        }

        internal record CompatData(int GameVersion, int[]? CompatGroup = null, bool IncludeInSupportRange = true);
    }
}
