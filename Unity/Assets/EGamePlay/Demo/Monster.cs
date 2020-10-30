﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay.Combat;
using UnityEngine.UI;
using DG.Tweening;

public sealed class Monster : MonoBehaviour
{
    public CombatEntity CombatEntity;
    public float MoveSpeed = 0.2f;
    public Text DamageText;
    public Image HealthBarImage;


    // Start is called before the first frame update
    void Start()
    {
        CombatEntity = new CombatEntity();
        CombatEntity.Initialize();
        CombatEntity.OnReceiveDamage += OnReceiveDamage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnReceiveDamage(DamageAction damageAction)
    {
        HealthBarImage.fillAmount = CombatEntity.Health.Percent();
        DamageText.text = damageAction.DamageValue.ToString();
        DamageText.GetComponent<DOTweenAnimation>().DORestart();
    }
}
