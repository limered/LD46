﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using Assets.Systems.Score;
using UniRx;

[GameSystem]
public class ThrowSystem : GameSystem<ThrowComponent, ScoreComponent>
{
    private readonly ReactiveProperty<ScoreComponent> _score = new ReactiveProperty<ScoreComponent>();

    public override void Register(ThrowComponent component)
    {
        component.ThrowTypes[0].gameObject.SetActive(false);
        component.ThrowTypes[1].gameObject.SetActive(false);
        component.ThrowTypes[2].gameObject.SetActive(false);
        component.WaitOn(_score, score => Register(score, component)).AddTo(component);
    }

    public override void Register(ScoreComponent component)
    {
        _score.Value = component;
    }

    public void Register(ScoreComponent score, ThrowComponent component)
    {
        score.HyperLevel.Subscribe(hyperLevel => SetParticleSystem(hyperLevel, component)).AddTo(component);
    }

    public void SetParticleSystem(HyperLevel hyperLevel, ThrowComponent component)
    {
        switch (hyperLevel)
        {
            case HyperLevel.Failing:
            case HyperLevel.Shitty:
                component.ThrowTypes[0].gameObject.SetActive(true);
                component.ThrowTypes[1].gameObject.SetActive(false);
                component.ThrowTypes[2].gameObject.SetActive(false);
                break;
            case HyperLevel.Cool:
                component.ThrowTypes[0].gameObject.SetActive(false);
                component.ThrowTypes[1].gameObject.SetActive(true);
                component.ThrowTypes[2].gameObject.SetActive(false);
                break;
            case HyperLevel.Hot:
                component.ThrowTypes[0].gameObject.SetActive(false);
                component.ThrowTypes[1].gameObject.SetActive(false);
                component.ThrowTypes[2].gameObject.SetActive(true);
                break;
        }
    }
}
