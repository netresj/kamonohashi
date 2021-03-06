# coding: utf-8

"""
    KAMONOHASHI API

    A platform for deep learning  # noqa: E501

    OpenAPI spec version: v2
    Contact: kamonohashi-support@jp.nssol.nipponsteel.com
    Generated by: https://github.com/swagger-api/swagger-codegen.git
"""


import pprint
import re  # noqa: F401

import six


class AccountApiModelsLoginInputModel(object):
    """NOTE: This class is auto generated by the swagger code generator program.

    Do not edit the class manually.
    """

    """
    Attributes:
      swagger_types (dict): The key is attribute name
                            and the value is attribute type.
      attribute_map (dict): The key is attribute name
                            and the value is json key in definition.
    """
    swagger_types = {
        'expires_in': 'int',
        'password': 'str',
        'tenant_id': 'int',
        'user_name': 'str'
    }

    attribute_map = {
        'expires_in': 'expiresIn',
        'password': 'password',
        'tenant_id': 'tenantId',
        'user_name': 'userName'
    }

    def __init__(self, expires_in=None, password=None, tenant_id=None, user_name=None):  # noqa: E501
        """AccountApiModelsLoginInputModel - a model defined in Swagger"""  # noqa: E501

        self._expires_in = None
        self._password = None
        self._tenant_id = None
        self._user_name = None
        self.discriminator = None

        if expires_in is not None:
            self.expires_in = expires_in
        self.password = password
        if tenant_id is not None:
            self.tenant_id = tenant_id
        self.user_name = user_name

    @property
    def expires_in(self):
        """Gets the expires_in of this AccountApiModelsLoginInputModel.  # noqa: E501


        :return: The expires_in of this AccountApiModelsLoginInputModel.  # noqa: E501
        :rtype: int
        """
        return self._expires_in

    @expires_in.setter
    def expires_in(self, expires_in):
        """Sets the expires_in of this AccountApiModelsLoginInputModel.


        :param expires_in: The expires_in of this AccountApiModelsLoginInputModel.  # noqa: E501
        :type: int
        """

        self._expires_in = expires_in

    @property
    def password(self):
        """Gets the password of this AccountApiModelsLoginInputModel.  # noqa: E501


        :return: The password of this AccountApiModelsLoginInputModel.  # noqa: E501
        :rtype: str
        """
        return self._password

    @password.setter
    def password(self, password):
        """Sets the password of this AccountApiModelsLoginInputModel.


        :param password: The password of this AccountApiModelsLoginInputModel.  # noqa: E501
        :type: str
        """
        if password is None:
            raise ValueError("Invalid value for `password`, must not be `None`")  # noqa: E501

        self._password = password

    @property
    def tenant_id(self):
        """Gets the tenant_id of this AccountApiModelsLoginInputModel.  # noqa: E501


        :return: The tenant_id of this AccountApiModelsLoginInputModel.  # noqa: E501
        :rtype: int
        """
        return self._tenant_id

    @tenant_id.setter
    def tenant_id(self, tenant_id):
        """Sets the tenant_id of this AccountApiModelsLoginInputModel.


        :param tenant_id: The tenant_id of this AccountApiModelsLoginInputModel.  # noqa: E501
        :type: int
        """

        self._tenant_id = tenant_id

    @property
    def user_name(self):
        """Gets the user_name of this AccountApiModelsLoginInputModel.  # noqa: E501


        :return: The user_name of this AccountApiModelsLoginInputModel.  # noqa: E501
        :rtype: str
        """
        return self._user_name

    @user_name.setter
    def user_name(self, user_name):
        """Sets the user_name of this AccountApiModelsLoginInputModel.


        :param user_name: The user_name of this AccountApiModelsLoginInputModel.  # noqa: E501
        :type: str
        """
        if user_name is None:
            raise ValueError("Invalid value for `user_name`, must not be `None`")  # noqa: E501

        self._user_name = user_name

    def to_dict(self):
        """Returns the model properties as a dict"""
        result = {}

        for attr, _ in six.iteritems(self.swagger_types):
            value = getattr(self, attr)
            if isinstance(value, list):
                result[attr] = list(map(
                    lambda x: x.to_dict() if hasattr(x, "to_dict") else x,
                    value
                ))
            elif hasattr(value, "to_dict"):
                result[attr] = value.to_dict()
            elif isinstance(value, dict):
                result[attr] = dict(map(
                    lambda item: (item[0], item[1].to_dict())
                    if hasattr(item[1], "to_dict") else item,
                    value.items()
                ))
            else:
                result[attr] = value
        if issubclass(AccountApiModelsLoginInputModel, dict):
            for key, value in self.items():
                result[key] = value

        return result

    def to_str(self):
        """Returns the string representation of the model"""
        return pprint.pformat(self.to_dict())

    def __repr__(self):
        """For `print` and `pprint`"""
        return self.to_str()

    def __eq__(self, other):
        """Returns true if both objects are equal"""
        if not isinstance(other, AccountApiModelsLoginInputModel):
            return False

        return self.__dict__ == other.__dict__

    def __ne__(self, other):
        """Returns true if both objects are not equal"""
        return not self == other
