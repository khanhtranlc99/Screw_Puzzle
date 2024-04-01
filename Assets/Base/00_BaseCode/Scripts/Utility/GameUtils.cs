using System;
using UniRx;
using UnityEngine;
using Frictionless;

public class GameUtils
{
    public static void RaiseMessage(object msg) {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.RaiseMessage(msg);
    }

    public static void AddHandler<T>(Action<T> handler)
    {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.AddHandler(handler);
    }

    public static void RemoveHandler<T>(Action<T> handler)
    {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.RemoveHandler(handler);
    }

    public static IObservable<T> AsObservable<T>() {
        return Observable.FromEvent<T>(AddHandler, RemoveHandler);
    }
}