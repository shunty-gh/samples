#pragma once

#include <string>

class FizzBuzz
{
public:
	FizzBuzz(int fizz, int buzz);
	~FizzBuzz();

	std::string const GetMessage(int value);

private:
	int _fizz = 3;
	int _buzz = 5;

	bool const IsFizz(int value);
	bool const IsBuzz(int value);
};