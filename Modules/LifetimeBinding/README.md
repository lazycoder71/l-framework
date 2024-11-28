[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE.md)

## Overview
In the Addressable Asset System, you need to explicitly release the loaded resources when they are no longer needed.

```cs
// Load.
var handle = Addressables.LoadAssetAsync<GameObject>("FooPrefab");
await handle.Task;

// Release.
Addressables.Release(handle);
```

If you forget to release, it can cause memory leaks and serious problems such as application crashes.
However, such an implementation is easy to forget to release, and it is difficult to notice when you forget it.

**Lifetime Binding** solves this problem by binding the lifetime of the loaded resource to a **GameObject**, for example, as follows.
When the associated **GameObject** is destroyed, the resource is automatically released.

```cs
var fooObj = new GameObject();

// Load and bind the lifetime of the resource to fooObj.
// The asset will be released at the same time the fooObj is destroyed.
Addressables.LoadAssetAsync<GameObject>("BarPrefab").BindTo(fooObj);
```

In the above code, the resource is released as soon as the **fooObj** is destroyed.

## Setup

### Requirement
* Unity 2020.3 or higher.
* Addressables is installed.

## Lifetime Binding
Lifetime Binding's basic features is lifetime binding.
This binds the lifetime of the resource to **GameObject** and so on, to release it automatically and reliably.

### Bind to GameObject
To bind the lifetime of the resource to **GameObject**, use the **BindTo** method as follows.

```cs
// Load the resource and bind the lifetime to gameObject.
var handle = Addressables
    .LoadAssetAsync<GameObject>("FooPrefab")
    .BindTo(gameObject);
await handle.Task;
var prefab = handle.Result;

// Destroy gameObject and release the resource.
Destroy(gameObject);
```

Now, the resource is released as soon as the gameObject is destroyed.

### Bind to non-GameObject
You can bind the lifetime to non-GameObject as well.
To do so, create a class that implements `IReleaseEvent` interface and pass it to `BindTo` method.