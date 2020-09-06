import request from './request'

// 获取文章类别
export const getNearUsers = (params) => {
  return request('get', `/Whisper/GetUserOnNearLogin`, params)
}
