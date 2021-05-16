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
            //Id�����߂܂��B
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            //�N�I���e�B�����߂܂�
            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleAction�̐ݒ� (�����Ő������̐ݒ蓙����H)
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
