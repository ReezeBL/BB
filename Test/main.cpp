#include <Windows.h>

typedef  FARPROC(WINAPI *LPMessageBox)(HWND, LPCWSTR, LPCWSTR, UINT);


int main(int argc, char* argv[])
{
	_asm{
		push 0xAAAAAAAA;
		pushfd;
		pushad;
		push 0xAAAAAAAA;
		mov eax, 0xAAAAAAAA;
		call eax;
		popad;
		popfd;
		ret;
	}
}
