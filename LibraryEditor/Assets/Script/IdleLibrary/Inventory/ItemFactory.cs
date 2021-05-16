using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    public class ArtifactFactory
    {
        public Artifact CreateArtifact()
        {
            var item = new Artifact(-1);
            //Idを決めます。
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            //クオリティを決めます
            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleActionの設定 (ここで成長率の設定等する？)
            var idleAction = new IdleAction(10);
            item.idleAction = idleAction;

            List<IEffect> effectList = new List<IEffect>();
            effectList.Add(new BasicEffect(BasicEffectKind.goldGain, () => item.level * 3, Calway.add));
            effectList.Add(new BasicEffect(BasicEffectKind.expGain, () => 1 + 0.1 + item.level * 0.1, Calway.mul));
            item.effects = effectList;

            return item;
        }
    }
}
