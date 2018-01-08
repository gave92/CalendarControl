# CalendarControl: Event calendar for UWP

![logo](https://img.shields.io/badge/license-MIT-blue.svg)

Calendar custom control for UWP platform. Supports week view and events.

## Installation

$ Install-Package Gave.Libs.CalendarControl

## Key features
* **Easy to use** (one XAML line for calendar and built-in event management)
* **Extensible** (you can modify look and functionality)

## Quick guide

#### 1. Basic usage. Default look and behaviour

```xml
<Page xmlns:calendar="using:CalendarControl">
  <Grid>
  <calendar:Calendar Padding="16,0,16,16"
                     CanvasMinHeight="500"
                     CanvasMaxHeight="600"
                     IsHourSelectionEnabled="True" />
  </Grid>
</Page
```

## Screenshots

<img src="https://github.com/gave92/CalendarControl/blob/master/Screenshots/Calendar_1.png?raw=true" width="750" />

© Copyright 2017 - 2018 by Marco Gavelli
