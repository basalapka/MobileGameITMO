using System;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.Netcode;
using UnityEngine;
using Unity.BossRoom.Gameplay.GameplayObjects.Character.AI;
using UnityEngine.UIElements;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    public class DamageReceiver : NetworkBehaviour, IDamageable
    {
        public event Action<ServerCharacter, int> DamageReceived;

        public event Action<Collision> CollisionEntered;
        //private PlayerServerCharacter m_PlayerServerCharacter;
        [SerializeField]
        NetworkLifeState m_NetworkLifeState;
        

        public void ReceiveHP(ServerCharacter inflicter, int HP)
        {
            //m_PlayerServerCharacter = GetComponent<PlayerServerCharacter>();
            //var m_activeCharacters = m_PlayerServerCharacter.GetPlayerServerCharacters();
            if (IsDamageable())
            {
                //ГНОМЫ ЗДЕСЬ ПОБЫВАЛИ
                DamageReceived?.Invoke(inflicter, HP);
                foreach (var character in PlayerServerCharacter.GetPlayerServerCharacters())
                {
                    if (character != inflicter) {
                        character.ReceiveHP(inflicter, HP);
                    }
                }
            }
        }

        public IDamageable.SpecialDamageFlags GetSpecialDamageFlags()
        {
            return IDamageable.SpecialDamageFlags.None;
        }

        public bool IsDamageable()
        {
            return m_NetworkLifeState.LifeState.Value == LifeState.Alive;
        }

        void OnCollisionEnter(Collision other)
        {
            CollisionEntered?.Invoke(other);
        }
    }
}
