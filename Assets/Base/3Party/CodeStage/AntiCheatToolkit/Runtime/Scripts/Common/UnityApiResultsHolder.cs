#region copyright
// -------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// -------------------------------------------------------
#endregion

#if !UNITY_WEBGL
#define ACTK_ASYNC
#endif

namespace CodeStage.AntiCheat.Common
{
	using Storage;
	using UnityEngine;

	/// <summary>
	/// User-friendly wrapper around few internally used Unity APIs which can't be accessed from background threads.
	/// </summary>
	/// You only need to touch this if you are going to use ObscuredFile / ObscuredFilePrefs from the background threads.
	public static class UnityApiResultsHolder
	{
		private static string deviceUniqueIdentifier;
		private static string persistentDataPath;

		/// <summary>
		/// Call this from the main thread before using ObscuredFile / ObscuredFilePrefs from the background threads.
		/// </summary>
		/// Calling this method avoids getting exceptions while working with ObscuredFile / ObscuredFilePrefs from the
		/// background threads.
		/// <param name="warmUpDeviceIdentifier">Pass true to init API needed for the Lock To Device feature
		/// (SystemInfo.deviceUniqueIdentifier).
		/// You need this to be true only when using ObscuredFile / ObscuredFilePrefs from the background threads
		/// with DeviceLock enabled and without custom DeviceID set.
		/// Passing true is similar to the DeviceIdHolder.ForceLockToDeviceInit() call, please read that API docs for
		/// more information about possible side effects.</param>
		public static void InitForAsyncUsage(bool warmUpDeviceIdentifier)
		{
			if (System.Threading.SynchronizationContext.Current == null)
			{
				Debug.LogError($"Please call {nameof(InitForAsyncUsage)} from main thread!");
				return;
			}

			GetPersistentDataPath();

			if (warmUpDeviceIdentifier)
				GetDeviceUniqueIdentifier();
		}

		internal static string GetDeviceUniqueIdentifier()
		{
			if (string.IsNullOrEmpty(deviceUniqueIdentifier))
			{
#if ACTK_ASYNC
				if (System.Threading.SynchronizationContext.Current != null)
					deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
				else
					throw new DeviceUniqueIdentifierException();
#else
				deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
#endif
			}
			
			return deviceUniqueIdentifier;
		}
		
		internal static string GetPersistentDataPath()
		{
			if (string.IsNullOrEmpty(persistentDataPath))
			{
#if ACTK_ASYNC
				if (System.Threading.SynchronizationContext.Current != null)
					persistentDataPath = Application.persistentDataPath;
				else
					throw new PersistentDataPathException();
#else
				persistentDataPath = Application.persistentDataPath;
#endif
			}
			
			return persistentDataPath;
		}
	}
}