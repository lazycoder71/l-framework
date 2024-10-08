# Vibration

Native plugin for Unity for iOS and Android.
Use custom vibrations on mobile.

Origin github repository: "https://github.com/BenoitFreslon/Vibration"

# Use

## Initialization

Initialize the plugin with this line before using vibrations:

`Vibration.Init();`

## Vibrations

### iOS and Android

#### Default vibration

Use `Vibration.Vibrate();` for a classic default ~400ms vibration

#### Pop vibration

Pop vibration: weak boom (For iOS: only available with the haptic engine. iPhone 6s minimum or Android)

`Vibration.VibratePop();`

#### Peek Vibration

Peek vibration: strong boom (For iOS: only available on iOS with the haptic engine. iPhone 6s minimum or Android)

`Vibration.VibratePeek();`

#### Nope Vibration

Nope vibration: series of three weak booms (For iOS: only available with the haptic engine. iPhone 6s minimum or Android)

`Vibration.VibrateNope();`

## Enable/Disable

Set `Vibration.Enable` to true or false.

---
## Android Only

#### Custom duration in milliseconds

`Vibration.Vibrate(500);` 

#### Pattern

```
long [] pattern = { 0, 1000, 1000, 1000, 1000 };
Vibration.Vibrate ( pattern, -1 );
```

#### Cancel

`Vibration.Cancel();`

---
## IOS only
vibration using haptic engine

`Vibration.VibrateIOS(ImpactFeedbackStyle.Light);`

`Vibration.VibrateIOS(ImpactFeedbackStyle.Medium);`

`Vibration.VibrateIOS(ImpactFeedbackStyle.Heavy);`

`Vibration.VibrateIOS(ImpactFeedbackStyle.Rigid);`

`Vibration.VibrateIOS(ImpactFeedbackStyle.Soft);`

`Vibration.VibrateIOS(NotificationFeedbackStyle.Error);`

`Vibration.VibrateIOS(NotificationFeedbackStyle.Success);`

`Vibration.VibrateIOS(NotificationFeedbackStyle.Warning);`

`Vibration.VibrateIOS_SelectionChanged();`