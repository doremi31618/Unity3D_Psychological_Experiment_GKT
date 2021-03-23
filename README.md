# Unity3D_Psychological_Experiment_GKT

<p align="center">
  <img width="100%" src="img/title_preview.jpg">
</p>

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

## SettingEnvironment 

### Hardware Requirements

1. Add HTC Add-on to your vive 

-- **vive** -- 
 * Step 1 : [Tutorial video](https://www.youtube.com/watch?v=Yu5XwwUHJKg)
 * Step 2 : [Tutorial video](https://www.youtube.com/watch?v=oGcPyWlEZS0)
 * Step 3 : [Tutorial video](https://www.youtube.com/watch?v=nIzuwHagIXQ)
 * Step 4 : [Tutorial video](https://www.youtube.com/watch?v=zswmKmIBrss)
 * Step 5 : Resemble it 
  
  
-- **vive pro** --

* Step 1 : [Tutorial video](https://www.youtube.com/watch?v=ZRdWlmxBH30&t=28s)

2. HTC vive installation

you can follow up this tutorial or just open the environment setting at SteamVR


### Software Requirements

1. HTC Vive setting 
  
please take a look at official tutorial from VIVE ([link](https://www.vive.com/tw/support/vive/category_howto/setting-up-for-the-first-time.html))

2. pupil lab capture
  
Because most of core function of this program base on the Pupil capture service ,  
you will need to open pupil capture and make sure you have correctly connect 
HMD Add-on directly to computer

## View-Preview
<h3>Title</h3>
<p align="center">
  <img width="100%" src="img/title_preview.jpg">
</p>

<h3>Checking Device</h3>
<p align="center">
  <img width="100%" src="img/checking_preview.jpg">
</p>


  

In this section you will need to check if all the device has settele down.  
first you need to finished the HTC Vive environment setting,  
next you will need to open the pupil capture software to get connect with  

if you haven't finish the environment setting you can back to the 
tutorial here ([link](#SettingEnvironment))


<h3>Main Menu</h3>
<p align="center">
  <img width="100%" src="img/mainmenu_preview.jpg">
</p>

<h3>Setting</h3>
<p align="center">
    <img width="100%" src="img/setting_preview.jpg">
</P>
### Terminology
1. gapTime :
2. maxTime : 
3. delayTime :
4. 

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
  * Custom plug-in
  * Pupil HMD Scripts : [link](https://github.com/pupil-labs/hmd-eyes/blob/master/docs/Developer.md#getting-started)

## Format

1. Experinment Setting :

2. Experiment Record :

3. Gaze Data : 

4. Pupil Export Data : Check the pupil document ([link](https://docs.pupil-labs.com/developer/core/recording-format/))

