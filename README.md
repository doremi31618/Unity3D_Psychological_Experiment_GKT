# Unity3D_Psychological_Experiment_GKT

## intro - English

This is a GKT (Guilty Knowledge Test) Experiment project integrate with Unity3D ,
VR and PupilLab

#### 1. Dev documentation : [(link)](#Developent)

if you are **Unity developer** and want to add some new features ,
you can see the overview page

#### 2. Data format intro : [(link)](#Format)

if you need to analize the exporting data,
you shold check the format introduction of data which created by program

#### 3. Preview : [(link)](#View-Preview)

check the program preview to get a better understanding of experiment flow



## View-Preview
<h3>Title</h3>
<p align="center">
  <img width="100%" src="img/title_preview.jpg">
</p>

<h3>Checking Device</h3>
<p align="center">
  <img width="100%" src="img/checking_preview.jpg">
</p>

In this section you will need to check if all the device has settele down . 
first you need to finished the HTC Vive environment setting ,
next you will need to open the pupil capture software 
to get connect with 

<h3>Main Menu</h3>
<p align="center">
  <img width="100%" src="img/mainmenu_preview.jpg">
</p>

<h3>Setting</h3>
<p align="center">
    <img width="100%" src="img/setting_preview.jpg">
</P>

<h3>Experiment</h3>
<p align="center">
  <img width="100%" src="img/Experiment_preview.jpg">
</p>

**1. page intro :**

At this page you can check the time sapn with (time bar) and the view what the subject see also the eye frame visualizer.

**2. Weak Eye and Strong Eye :**  

In this preview the left eye is weak eye and right eye is strong eye.
**Weak Eye** has four visual target and will automatically start fade in when FadeIn Stage start , 
**Strong Eye** has only one video which is mondrian video (produced by this [project](https://github.com/doremi31618/Processing_MondrianPatternGenerator)
 have a look if you need some mondrian video sources)

the visual target image source can change by modifing the visual target path at [setting page](#Setting)
the mondiran video source can also change by modifying the mondiran path at [setting page](#Setting)
both can be found at StreammingAssets path


**3. Experiment Stage :**

* PupilLab Calibration (only execute at very first time) 
* Data Optimize Calibration
* Start Delay : modify the time length 
* FadeIn
* End Delay



## Developent

the program can be split to three parts

1. GKT-Experiment : control the experiment process
2. GameManager : Control the UI Event and the interface between GKT-Experiment and GUI Page
3. EyeTracker : 

## Format
