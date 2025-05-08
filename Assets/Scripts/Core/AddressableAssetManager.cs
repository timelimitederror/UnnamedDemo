using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAssetManager : MonoBehaviourSinqletonBase<AddressableAssetManager>
{
    private Dictionary<string, AssetLoader> assetLoaders = new Dictionary<string, AssetLoader>();

    public void loadAsset(string address, Action<object> action)
    {
        if (assetLoaders.ContainsKey(address))
        {
            action.Invoke(assetLoaders[address].result);
        }
        assetLoaders[address] = new AssetLoader(address, action);
    }

    public void releaseAsset(string address)
    {
        if (assetLoaders.ContainsKey(address))
        {
            assetLoaders[address].release();
            assetLoaders.Remove(address);
        }
    }

    public GameObject instantiateGameObject(GameObject obj)
    {
        return Instantiate(obj);
    }

    public void destroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }

    private class AssetLoader
    {
        public string address;
        public AsyncOperationHandle handle;
        public Action<object> action;
        public object result;

        public AssetLoader(string address, Action<object> action)
        {
            this.address = address;
            this.action = action;
            this.handle = Addressables.LoadAssetAsync<object>(address);
            handle.Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    result = handle.Result;
                    action.Invoke(handle.Result);
                }
            };
        }

        public void release()
        {
            Addressables.Release(handle);
        }
    }
}
