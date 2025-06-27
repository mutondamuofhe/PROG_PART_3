Cybersecurity Awareness Assistant
Project Details
Project Name: PROG_PART_3

.NET Version: .NET 8.0 (LTS)

Template: WPF App (C#)

This project is a WPF desktop application developed to teach users about cybersecurity through an interactive assistant. It features a chatbot that responds to user queries, a task manager for cybersecurity to-dos, a quiz game to test knowledge, and an activity log—all within a visually engaging GUI with sound and image elements.

Features
Chat Assistant:
Responds to user input using simulated keyword-based NLP for topics like cybersecurity risks, quiz, or task reminders.

Voice Greeting:
Automatically plays a greeting sound (voice_greeting.wav) when the application launches.

Task Manager:
Allows users to add, delete, and complete cybersecurity-related tasks. Tasks may also include reminders.

Security Quiz:
Offers a multiple-choice quiz with instant feedback and a score summary to help users test their cybersecurity knowledge.

Activity Log:
Logs chatbot responses, tasks, and quiz activities for user review.

Modern UI with Navigation Tabs:
GUI includes sections for Chat Assistant, Task Manager, Security Quiz, and Activity Log, all accessible through a tab-like button interface.

Background Image:
Uses a semi-transparent image (LOGO.jpg) as the background to enhance visual appeal.

Classes and Functionality
1. MainWindow Class (MainWindow.xaml.cs)
This is the main code-behind file that manages all interactions in the WPF interface.

Key Functions:
Initializes all four panels (Chat, Task, Quiz, Log) on load.

Listens for button clicks and keyboard events.

Routes input to appropriate handlers (e.g., quiz start, chat reply, task creation).

Code Highlights:
SendMessage(): Processes user chat input.

ProcessUserInput(): Routes keywords to chatbot, quiz, task, or log features.

AddChatMessage(): Displays chat text with timestamps.

ExtractTaskFromMessage(): Parses tasks from user input (e.g., "remind me to update antivirus").

2. XAML File (MainWindow.xaml)
Defines the layout and design of the application using WPF controls.

Key UI Sections:
ChatPanel: Scrollable chat history and input box.

TaskPanel: ListView of current tasks, task creation area.

QuizPanel: Quiz game with question, options, feedback, and results.

LogPanel: ListView of activity history using data templates and color coding.

Visual Features:
Custom buttons and color themes.

Responsive layout with grid and stack panels.

Uses ZIndex to layer background image and content.

3. Quiz Functionality
Loads a set of quiz questions from a backend list.

Displays each question with options.

Provides immediate feedback after each answer.

Shows total score and a message based on performance.

4. Task Management
Adds tasks with optional reminders.

Tasks can be completed or deleted.

List updates dynamically using ObservableCollection.

5. Activity Log
Stores interactions and important events.

Displays time-stamped logs using color-coded tags for clarity.

How to Run
Clone or download this project to your local machine.

Ensure the following resource files exist in the project directory:

voice_greeting.wav (for sound greeting)

LOGO.jpg (for background image)

Open the solution in Visual Studio 2022 or later.

Make sure .NET 8.0 SDK is installed.

Build the project using Ctrl + Shift + B.

Run the program using F5.

Example Interactions

Chat Assistant
You: remind me to update my firewall  
Bot: I've prepared a task with the title 'Update my firewall'. Please add any additional details in the Task Manager tab.

Quiz Game
You: I want to take a quiz  
Bot: I've opened the Cybersecurity Knowledge Quiz for you. Click 'Start Quiz' when you're ready!

Activity Log
You: show my log  
Bot: I've opened the activity log for you to review.