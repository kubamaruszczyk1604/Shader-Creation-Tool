
#include "Stopwatch.h"
using namespace std::chrono;

Stopwatch::Stopwatch() :isRunning(0), isPaused(0),
m_Elapsed_seconds(std::chrono::duration<double>(0)),
m_Paused_seconds(std::chrono::duration<double>(0))
{

}


Stopwatch::~Stopwatch()
{

}

void Stopwatch::Start()
{
	if (!isRunning)
	{
		m_Start = high_resolution_clock::now();
		m_Elapsed_seconds = std::chrono::duration<double>(0);
		m_Paused_seconds = std::chrono::duration<double>(0);
		isPaused = false;
	}
	isRunning = true;
}

void Stopwatch::Stop()
{
	if (!isRunning) return;
	m_End = high_resolution_clock::now();
	this->UnPause();
	isRunning = false;

}

void Stopwatch::UnPause()
{
	if (!isPaused) return;
	if (!isRunning) return;

	isPaused = false;
	m_Paused_seconds += duration<double>(high_resolution_clock::now() - m_PauseStart);

}
void Stopwatch::Pause()
{
	if (isPaused) return;
	if (!isRunning) return;
	isPaused = true;
	m_PauseStart = high_resolution_clock::now();


}


double Stopwatch::ElapsedTime()const
{

	if (isRunning)
	{
		if (isPaused)
		{
			// Duration between Start point and last pause, minus time sum of previous pause periods 
			return (duration_cast<duration<double>>(m_PauseStart - m_Start) - m_Paused_seconds).count();
		}
		time_point<high_resolution_clock> localEnd = high_resolution_clock::now();
		return (duration_cast<duration<float>>(localEnd - m_Start) - m_Paused_seconds).count();
	}
	else
	{
		return (duration_cast<duration<float>>(m_End - m_Start) - m_Paused_seconds).count();
	}
}