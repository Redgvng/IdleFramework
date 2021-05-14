using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary.Inventory;

namespace IdleLibrary.Inventory
{
    public interface ISetItem
    {
        void SetItem(ITEM itme, int index);
    }

    public class SimpleSetItem : ISetItem
    {
        private readonly Inventory inventory;
        public SimpleSetItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void SetItem(ITEM item, int index)
        {
            inventory.items[index] = item;
        }
    }
}

//IEH1用なのでクラスをArtifactで限定させる。汎用的にする必要はない
public class StartIdleActionWithSet : ISetItem
{
    private readonly ISetItem setItem;
    public StartIdleActionWithSet(ISetItem setItem)
    {
        this.setItem = setItem;
    }
    public void SetItem(ITEM item, int index)
    {
        setItem.SetItem(item, index);
        Debug.Log("呼んでるよ");
        if (item is Artifact)
        {
            Debug.Log("yobaretreuyo");
            var artifact = item as Artifact;
            artifact.idleAction.Initialize();
            artifact.idleAction.Start();
        }
    }
}