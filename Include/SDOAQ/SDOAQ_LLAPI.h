/* SDOAQ_LOWLEVEL.h

	Comments : This file exports all types and functions needed to access the SDO acquisition engine.
	Date     : 2023/08/25
	Author   : YoungJu Lee
	Copyright (c) 2019 SD Optics,Inc. All rights reserved.

	========================================================================================================================================================
	Revision history
	========================================================================================================================================================
	Version     date      Author         Descriptions
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.3.0  2022.08.25  YoungJu Lee     - Init
	--------------------------------------------------------------------------------------------------------------------------------------------------------
*/

#pragma once
#include "SDOAQ_WS.h"

#ifdef __cplusplus
extern "C"
{
#endif	
	
	// low level API ��� ������ ����Ѵ�.
	__declspec(dllexport) void SDOAQ_RegisterLowLevelAuthorization(void);


	// 'pAcquisitionParams'�� �������� ī�޶� �����Ѵ�.
	__declspec(dllexport) eErrorCode SDOAQ_SetAcquisitionFixedParameters(sAcquisitionFixedParameters* pAcquisitionParams);


	// ['FrameDescriptor' structure]
	//		'typeThis': 'FrameDescriptor'�� ����. �� ���� �׻� 1�̴�.
	//		'bytesPixel': Frame�� �� �ȼ��� �̷�� ������ ����Ʈ ��.
	//		'pixelsWidth': Frame�� ���� �ȼ� ��
	//		'pixelsHeight': Frame�� ���� �ȼ� ��
	//		'bytesLine': Frame�� ���� ���� ������ ����Ʈ. ���⿡�� �е� ����Ʈ�� ���Ե� �� �ִ� ( padding bytes = 'bytesLine' - 'bytesPixel' * 'pixelsWidth').
	typedef struct
	{
		int typeThis;
		int bytesPixel;
		int pixelsWidth;
		int pixelsHeight;
		int bytesLine;
	} FrameDescriptor;

	// 'SDOAQ_FrameCallback' �ݹ� �Լ�
	//		'errorCode': ecNoError
	//		'pBuffer': ������ ���� ������. �ݹ��Լ��� �����ϸ� ���۴� �� �̻� ��ȿ���� �����Ƿ� �ݹ� �Լ� �ȿ��� �����͸� ����ؾ� �Ѵ�.
	//		'BufferSize': 'pBuffer'�� ���� ũ��
	//		'pFrameDescriptor': 'pBuffer'�� ���ۿ� ��� ������ ����, �� ���� NULL�̸� ������ ���� ������.
	typedef void(__stdcall* SDOAQ_FrameCallback)(eErrorCode errorCode, unsigned char* pBuffer, size_t BufferSize, FrameDescriptor* pFrameDescriptor);
	// ������ �����͸� �޴� �ݹ��Լ��� ����Ѵ�: 'singleFrameCb'�� NULL�� ���� �ݹ��� ���� �ʴ´�.
	__declspec(dllexport) eErrorCode SDOAQ_SetFrameCallback(SDOAQ_FrameCallback singleFrameCb);


	enum eCameraTriggerMode
	{
		ctmFreerun = 1,
		ctmSoftware = 2,
		ctmExternal = 3
	};
	// ī�޶� Ʈ���� ��带 �����Ѵ�.
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraTriggerMode(eCameraTriggerMode ctm);


	// �Ʒ� API���� �Ϻ� ī�޶󿡼��� ����ȴ�.
	enum eCameraParameterType
	{
		cptValue = 0,
		cptMin = 1,
		cptMax = 2,
		cptInc = 3,
	};

	__declspec(dllexport) eErrorCode SDOAQ_ExecCameraSoftwareTrigger(void);
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraParameterString(const char* sz_register, const char* sz_value);
	__declspec(dllexport) eErrorCode SDOAQ_GetCameraParameterString(const char* sz_register, char* buffer_ptr, int buffer_size);
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraParameterInteger(const char* sz_register, long long value);
	__declspec(dllexport) eErrorCode SDOAQ_GetCameraParameterInteger(const char* sz_register, long long* value_ptr, eCameraParameterType cpt);
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraParameterDouble(const char* sz_register, double value);
	__declspec(dllexport) eErrorCode SDOAQ_GetCameraParameterDouble(const char* sz_register, double* value_ptr, eCameraParameterType cpt);
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraParameterBool(const char* sz_register, bool value);
	__declspec(dllexport) eErrorCode SDOAQ_GetCameraParameterBool(const char* sz_register, bool* value_ptr);

#ifdef __cplusplus
}
#endif