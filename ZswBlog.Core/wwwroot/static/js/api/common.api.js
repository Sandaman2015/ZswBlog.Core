import { requestCors } from './request'

export const getIp = (params) => {
  return requestCors('get', `https://apis.map.qq.com/ws/location/v1/ip?key=HIQBZ-WNQKK-6VZJ7-AR3XK-HEVAK-5DBXD`, params)
}

